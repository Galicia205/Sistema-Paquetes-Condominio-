using System.Collections.Generic;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Interfaces
{
    public interface IPaqueteRepository
    {
        List<Paquete> ObtenerTodos();

        int Crear(Paquete paquete);

        bool MarcarComoNotificado(int paqueteId);
    }
}