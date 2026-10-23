using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Repositories
{
    public class PaqueteRepository : IPaqueteRepository
    {
        private readonly string conexion =
            ConfigurationManager.ConnectionStrings["ConexionPaquetes"].ConnectionString;

        public List<Paquete> ObtenerTodos()
        {
            List<Paquete> paquetes = new List<Paquete>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT p.PaqueteId, p.Descripcion, p.Estado,
                           p.ResidenteId, r.Nombre AS NombreResidente,
                           rec.FechaHoraRecepcion, rec.RegistradoPor
                    FROM Paquetes p
                    INNER JOIN Residentes r
                        ON p.ResidenteId = r.ResidenteId
                    INNER JOIN Recepciones rec
                        ON p.PaqueteId = rec.PaqueteId
                    ORDER BY rec.FechaHoraRecepcion DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paquetes.Add(new Paquete
                            {
                                PaqueteId = Convert.ToInt32(reader["PaqueteId"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Estado = reader["Estado"].ToString(),
                                ResidenteId = Convert.ToInt32(reader["ResidenteId"]),
                                NombreResidente = reader["NombreResidente"].ToString(),
                                FechaHoraRecepcion = Convert.ToDateTime(
                                    reader["FechaHoraRecepcion"]),
                                RegistradoPor = reader["RegistradoPor"].ToString()
                            });
                        }
                    }
                }
            }

            return paquetes;
        }

        public int Crear(Paquete paquete)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                using (SqlTransaction transaccion = cn.BeginTransaction())
                {
                    try
                    {
                        int paqueteId;

                        string sqlPaquete = @"
                            INSERT INTO Paquetes
                                (Descripcion, Estado, ResidenteId)
                            VALUES
                                (@Descripcion, 'Recibido', @ResidenteId);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmd = new SqlCommand(
                            sqlPaquete, cn, transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@Descripcion", paquete.Descripcion);
                            cmd.Parameters.AddWithValue(
                                "@ResidenteId", paquete.ResidenteId);

                            paqueteId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string sqlRecepcion = @"
                            INSERT INTO Recepciones
                                (PaqueteId, FechaHoraRecepcion, RegistradoPor)
                            VALUES
                                (@PaqueteId, GETDATE(), @RegistradoPor);";

                        using (SqlCommand cmd = new SqlCommand(
                            sqlRecepcion, cn, transaccion))
                        {
                            cmd.Parameters.AddWithValue("@PaqueteId", paqueteId);
                            cmd.Parameters.AddWithValue(
                                "@RegistradoPor", paquete.RegistradoPor);

                            cmd.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return paqueteId;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
        public bool MarcarComoNotificado(int paqueteId)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = @"
            UPDATE Paquetes
            SET Estado = 'Notificado'
            WHERE PaqueteId = @PaqueteId
              AND Estado = 'Recibido'";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PaqueteId", paqueteId);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    return filasAfectadas == 1;
                }
            }
        }
    }
}