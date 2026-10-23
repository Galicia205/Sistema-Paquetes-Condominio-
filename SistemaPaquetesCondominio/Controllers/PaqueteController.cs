using System.Linq;
using System.Web.Mvc;
using SistemaPaquetesCondominio.Interfaces;
using SistemaPaquetesCondominio.Models;
using SistemaPaquetesCondominio.Repositories;
using SistemaPaquetesCondominio.Services;

namespace SistemaPaquetesCondominio.Controllers
{
    public class PaqueteController : Controller
    {
        private readonly PaqueteService _service;
        private readonly IResidenteRepository _residenteRepository;

        public PaqueteController()
        {
            IPaqueteRepository repository = new PaqueteRepository();
            _service = new PaqueteService(repository);

            _residenteRepository = new ResidenteRepository();
        }

        public ActionResult Index()
        {
            var paquetes = _service.ObtenerTodos();
            return View(paquetes);
        }

        public ActionResult Crear()
        {
            CargarResidentes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Paquete paquete)
        {
            var residentes = _residenteRepository.ObtenerTodos();

            bool residenteExiste = residentes.Any(
                r => r.ResidenteId == paquete.ResidenteId);

            if (string.IsNullOrWhiteSpace(paquete.Descripcion))
            {
                ModelState.AddModelError(
                    "Descripcion",
                    "Escribe la descripción del paquete.");
            }

            if (string.IsNullOrWhiteSpace(paquete.RegistradoPor))
            {
                ModelState.AddModelError(
                    "RegistradoPor",
                    "Escribe el nombre de quien recibe el paquete.");
            }

            if (!residenteExiste)
            {
                ModelState.AddModelError(
                    "ResidenteId",
                    "Selecciona un residente válido.");
            }

            if (!ModelState.IsValid)
            {
                CargarResidentes();
                return View(paquete);
            }

            _service.Crear(paquete);

            return RedirectToAction("Index");
        }

        private void CargarResidentes()
        {
            var residentes = _residenteRepository.ObtenerTodos();

            ViewBag.Residentes = new SelectList(
                residentes,
                "ResidenteId",
                "Nombre"
            );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Notificar(int id)
        {
            bool actualizado = _service.MarcarComoNotificado(id);

            if (!actualizado)
            {
                TempData["Mensaje"] =
                    "El paquete no existe o ya no está en estado Recibido.";
            }
            else
            {
                TempData["Mensaje"] =
                    "El paquete se marcó como Notificado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}