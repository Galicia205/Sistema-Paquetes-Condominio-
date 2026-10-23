using System.Collections.Generic;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Interfaces
{
    public interface IRetiroRepository
    {
        List<Retiro> ObtenerTodos();

        bool Registrar(Retiro retiro);
    }
}