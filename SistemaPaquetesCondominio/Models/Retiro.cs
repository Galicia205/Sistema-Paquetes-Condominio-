using System;

namespace SistemaPaquetesCondominio.Models
{
    public class Retiro
    {
        public int RetiroId { get; set; }

        public int PaqueteId { get; set; }

        public DateTime FechaHoraRetiro { get; set; }

        public string RecibidoPor { get; set; }

        public string DescripcionPaquete { get; set; }

        public string NombreResidente { get; set; }
    }
}