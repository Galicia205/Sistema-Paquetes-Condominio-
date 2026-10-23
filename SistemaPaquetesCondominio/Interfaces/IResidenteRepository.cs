using System.Collections.Generic;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Interfaces
{
    public interface IResidenteRepository
    {
        List<Residente> ObtenerTodos();

        int Crear(Residente residente);
    }
}