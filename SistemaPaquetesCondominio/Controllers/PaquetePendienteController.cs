using System.Web.Mvc;
using SistemaPaquetesCondominio.Repositories;
using SistemaPaquetesCondominio.Services;

namespace SistemaPaquetesCondominio.Controllers
{
    public class PaquetePendienteController : Controller
    {
        private readonly PaquetePendienteService _service;

        public PaquetePendienteController()
        {
            _service = new PaquetePendienteService(
                new PaquetePendienteRepository());
        }

        public ActionResult Index(string nombreResidente)
        {
            var paquetes = _service.BuscarPorResidente(
                nombreResidente);

            ViewBag.NombreResidente = nombreResidente;

            return View(paquetes);
        }
    }
}