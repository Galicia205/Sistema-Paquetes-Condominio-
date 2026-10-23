using System.Collections.Generic;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Services
{
    public class PaqueteService
    {
        private readonly IPaqueteRepository _repository;

        public PaqueteService(IPaqueteRepository repository)
        {
            _repository = repository;
        }

        public List<Paquete> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public int Crear(Paquete paquete)
        {
            return _repository.Crear(paquete);
        }
        public bool MarcarComoNotificado(int paqueteId)
        {
            return _repository.MarcarComoNotificado(paqueteId);
        }
    }
}