using System.Linq;
using System.Web.Mvc;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;
using SistemaPaquetesCondominio.Repositories;
using SistemaPaquetesCondominio.Services;

namespace SistemaPaquetesCondominio.Controllers
{
    public class ResidenteController : Controller
    {
        private readonly ResidenteService _service;
        private readonly IViviendaRepository _viviendaRepository;

        public ResidenteController()
        {
            IResidenteRepository repository = new ResidenteRepository();
            _service = new ResidenteService(repository);

            _viviendaRepository = new ViviendaRepository();
        }

        public ActionResult Index()
        {
            var residentes = _service.ObtenerTodos();
            return View(residentes);
        }

        public ActionResult Crear()
        {
            CargarViviendas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Residente residente)
        {
            var viviendaExiste = _viviendaRepository
                .ObtenerTodas()
                .Any(v => v.ViviendaId == residente.ViviendaId);

            if (!ModelState.IsValid || !viviendaExiste)
            {
                if (!viviendaExiste)
                {
                    ModelState.AddModelError(
                        "ViviendaId",
                        "Selecciona una vivienda válida.");
                }

                CargarViviendas();
                return View(residente);
            }

            _service.Crear(residente);

            return RedirectToAction("Index");
        }

        private void CargarViviendas()
        {
            var viviendas = _viviendaRepository.ObtenerTodas();

            ViewBag.Viviendas = new SelectList(
                viviendas.Select(v => new
                {
                    v.ViviendaId,
                    Descripcion = v.Numero + " - Bloque " + v.Bloque
                }),
                "ViviendaId",
                "Descripcion"
            );
        }
    }
}