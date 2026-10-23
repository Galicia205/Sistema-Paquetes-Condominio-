using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Repositories
{
    public class RetiroRepository : IRetiroRepository
    {
        private readonly string conexion =
            ConfigurationManager.ConnectionStrings[
                "ConexionPaquetes"].ConnectionString;

        public List<Retiro> ObtenerTodos()
        {
            var retiros = new List<Retiro>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT r.RetiroId,
                           r.PaqueteId,
                           r.FechaHoraRetiro,
                           r.RecibidoPor,
                           p.Descripcion AS DescripcionPaquete,
                           res.Nombre AS NombreResidente
                    FROM Retiros r
                    INNER JOIN Paquetes p
                        ON r.PaqueteId = p.PaqueteId
                    INNER JOIN Residentes res
                        ON p.ResidenteId = res.ResidenteId
                    ORDER BY r.FechaHoraRetiro DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retiros.Add(new Retiro
                            {
                                RetiroId = (int)reader["RetiroId"],
                                PaqueteId = (int)reader["PaqueteId"],
                                FechaHoraRetiro =
                                    (System.DateTime)reader["FechaHoraRetiro"],
                                RecibidoPor = reader["RecibidoPor"].ToString(),
                                DescripcionPaquete =
                                    reader["DescripcionPaquete"].ToString(),
                                NombreResidente =
                                    reader["NombreResidente"].ToString()
                            });
                        }
                    }
                }
            }

            return retiros;
        }

        public bool Registrar(Retiro retiro)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                using (SqlTransaction transaccion = cn.BeginTransaction())
                {
                    try
                    {
                        string sqlRetiro = @"
                            INSERT INTO Retiros
                                (PaqueteId, FechaHoraRetiro, RecibidoPor)
                            SELECT @PaqueteId, GETDATE(), @RecibidoPor
                            WHERE EXISTS
                            (
                                SELECT 1
                                FROM Paquetes
                                WHERE PaqueteId = @PaqueteId
                                  AND Estado = 'Notificado'
                            )
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM Retiros
                                WHERE PaqueteId = @PaqueteId
                            )";

                        int filasAfectadas;

                        using (SqlCommand cmd = new SqlCommand(
                            sqlRetiro, cn, transaccion))
                        {
                            cmd.Parameters.Add("@PaqueteId",
                                SqlDbType.Int).Value = retiro.PaqueteId;

                            cmd.Parameters.Add("@RecibidoPor",
                                SqlDbType.NVarChar, 100).Value =
                                retiro.RecibidoPor.Trim();

                            filasAfectadas = cmd.ExecuteNonQuery();
                        }

                        if (filasAfectadas != 1)
                        {
                            transaccion.Rollback();
                            return false;
                        }

                        string sqlEstado = @"
                            UPDATE Paquetes
                            SET Estado = 'Retirado'
                            WHERE PaqueteId = @PaqueteId
                              AND Estado = 'Notificado'";

                        using (SqlCommand cmd = new SqlCommand(
                            sqlEstado, cn, transaccion))
                        {
                            cmd.Parameters.Add("@PaqueteId",
                                SqlDbType.Int).Value = retiro.PaqueteId;

                            if (cmd.ExecuteNonQuery() != 1)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}