using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Repositories
{
    public class ViviendaRepository : IViviendaRepository
    {
        private readonly string conexion =
            ConfigurationManager.ConnectionStrings["ConexionPaquetes"].ConnectionString;

        public List<Vivienda> ObtenerTodas()
        {
            List<Vivienda> viviendas = new List<Vivienda>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT ViviendaId, Numero, Bloque
                    FROM Viviendas
                    ORDER BY Numero";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            viviendas.Add(new Vivienda
                            {
                                ViviendaId = (int)reader["ViviendaId"],
                                Numero = reader["Numero"].ToString(),
                                Bloque = reader["Bloque"] == System.DBNull.Value
                                    ? null
                                    : reader["Bloque"].ToString()
                            });
                        }
                    }
                }
            }

            return viviendas;
        }
    }
}