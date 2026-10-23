using System.Collections.Generic;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Interfaces
{
    public interface IPaquetePendienteRepository
    {
        List<PaquetePendiente> BuscarPorResidente(string nombreResidente);
    }
}