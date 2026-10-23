using System;

namespace SistemaPaquetesCondominio.Models
{
    public class Paquete
    {
        public int PaqueteId { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }

        public int ResidenteId { get; set; }

        public string NombreResidente { get; set; }

        public DateTime FechaHoraRecepcion { get; set; }

        public string RegistradoPor { get; set; }
    }
}