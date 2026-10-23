using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Repositories
{
    public class PaquetePendienteRepository
        : IPaquetePendienteRepository
    {
        private readonly string conexion =
            ConfigurationManager.ConnectionStrings[
                "ConexionPaquetes"].ConnectionString;

        public List<PaquetePendiente> BuscarPorResidente(
            string nombreResidente)
        {
            var paquetes = new List<PaquetePendiente>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT
                        p.PaqueteId,
                        p.Descripcion,
                        r.Nombre AS NombreResidente,
                        v.Numero AS NumeroVivienda,
                        rec.FechaHoraRecepcion,
                        p.Estado
                    FROM Paquetes p
                    INNER JOIN Residentes r
                        ON p.ResidenteId = r.ResidenteId
                    INNER JOIN Viviendas v
                        ON r.ViviendaId = v.ViviendaId
                    INNER JOIN Recepciones rec
                        ON p.PaqueteId = rec.PaqueteId
                    WHERE p.Estado IN ('Recibido', 'Notificado')
                      AND r.Nombre LIKE @NombreResidente
                    ORDER BY rec.FechaHoraRecepcion DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue(
                        "@NombreResidente",
                        "%" + (nombreResidente ?? "").Trim() + "%");

                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paquetes.Add(new PaquetePendiente
                            {
                                PaqueteId =
                                    (int)reader["PaqueteId"],

                                Descripcion =
                                    reader["Descripcion"].ToString(),

                                NombreResidente =
                                    reader["NombreResidente"].ToString(),

                                NumeroVivienda =
                                    reader["NumeroVivienda"].ToString(),

                                FechaHoraRecepcion =
                                    (System.DateTime)reader[
                                        "FechaHoraRecepcion"],

                                Estado =
                                    reader["Estado"].ToString()
                            });
                        }
                    }
                }
            }

            return paquetes;
        }
    }
}