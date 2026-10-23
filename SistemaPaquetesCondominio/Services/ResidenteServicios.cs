using System.Collections.Generic;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Services
{
    public class ResidenteService
    {
        private readonly IResidenteRepository _repository;

        public ResidenteService(IResidenteRepository repository)
        {
            _repository = repository;
        }

        public List<Residente> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public int Crear(Residente residente)
        {
            return _repository.Crear(residente);
        }
    }
}