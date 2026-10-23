using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Repositories
{
    public class ResidenteRepository : IResidenteRepository
    {
        private readonly string conexion =
            ConfigurationManager.ConnectionStrings["ConexionPaquetes"].ConnectionString;

        public List<Residente> ObtenerTodos()
        {
            List<Residente> residentes = new List<Residente>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT r.ResidenteId, r.Nombre, r.Telefono,
                           r.Correo, r.ViviendaId, v.Numero AS NumeroVivienda
                    FROM Residentes r
                    INNER JOIN Viviendas v
                        ON r.ViviendaId = v.ViviendaId
                    ORDER BY r.Nombre";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            residentes.Add(new Residente
                            {
                                ResidenteId = Convert.ToInt32(reader["ResidenteId"]),
                                Nombre = reader["Nombre"].ToString(),
                                Telefono = reader["Telefono"] == DBNull.Value
                                    ? null : reader["Telefono"].ToString(),
                                Correo = reader["Correo"] == DBNull.Value
                                    ? null : reader["Correo"].ToString(),
                                ViviendaId = Convert.ToInt32(reader["ViviendaId"]),
                                NumeroVivienda = reader["NumeroVivienda"].ToString()
                            });
                        }
                    }
                }
            }

            return residentes;
        }

        public int Crear(Residente residente)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    INSERT INTO Residentes
                        (Nombre, Telefono, Correo, ViviendaId)
                    VALUES
                        (@Nombre, @Telefono, @Correo, @ViviendaId);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", residente.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono",
                        (object)residente.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Correo",
                        (object)residente.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ViviendaId", residente.ViviendaId);

                    cn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}