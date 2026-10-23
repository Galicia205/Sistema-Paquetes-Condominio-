using System.Collections.Generic;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;

namespace SistemaPaquetesCondominio.Services
{
    public class PaquetePendienteService
    {
        private readonly IPaquetePendienteRepository _repository;

        public PaquetePendienteService(
            IPaquetePendienteRepository repository)
        {
            _repository = repository;
        }

        public List<PaquetePendiente> BuscarPorResidente(
            string nombreResidente)
        {
            if (string.IsNullOrWhiteSpace(nombreResidente))
            {
                return new List<PaquetePendiente>();
            }

            return _repository.BuscarPorResidente(
                nombreResidente.Trim());
        }
    }
}