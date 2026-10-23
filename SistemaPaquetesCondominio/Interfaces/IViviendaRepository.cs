using System.Collections.Generic;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Interfaces
{
    public interface IViviendaRepository
    {
        List<Vivienda> ObtenerTodas();
    }
}