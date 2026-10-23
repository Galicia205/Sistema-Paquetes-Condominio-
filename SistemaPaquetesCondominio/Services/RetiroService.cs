using System.Collections.Generic;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Services
{
    public class RetiroService
    {
        private readonly IRetiroRepository _repository;

        public RetiroService(IRetiroRepository repository)
        {
            _repository = repository;
        }

        public List<Retiro> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public bool Registrar(Retiro retiro)
        {
            if (retiro == null)
            {
                return false;
            }

            if (retiro.PaqueteId <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(retiro.RecibidoPor))
            {
                return false;
            }

            return _repository.Registrar(retiro);
        }
    }
}