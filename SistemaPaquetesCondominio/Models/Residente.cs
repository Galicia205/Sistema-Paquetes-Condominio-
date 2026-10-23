using System;

namespace SistemaPaquetesCondominio.Models
{
    public class Residente
    {
        public int ResidenteId { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public int ViviendaId { get; set; }

        // Datos de la vivienda relacionada
        public string NumeroVivienda { get; set; }
    }
}