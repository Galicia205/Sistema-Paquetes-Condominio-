using System;

namespace SistemaPaquetesCondominio.Models
{
    public class PaquetePendiente
    {
        public int PaqueteId { get; set; }

        public string Descripcion { get; set; }

        public string NombreResidente { get; set; }

        public string NumeroVivienda { get; set; }

        public DateTime FechaHoraRecepcion { get; set; }

        public string Estado { get; set; }
    }
}