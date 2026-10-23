using System.Web.Mvc;
using SistemaPaquetesCondominio.Models;
using SistemaPaquetesCondominio.Repositories;
using SistemaPaquetesCondominio.Services;

namespace SistemaPaquetesCondominio.Controllers
{
    public class RetiroController : Controller
    {
        private readonly RetiroService _service;
        private readonly PaqueteRepository _paqueteRepository;

        public RetiroController()
        {
            _service = new RetiroService(new RetiroRepository());
            _paqueteRepository = new PaqueteRepository();
        }

        public ActionResult Index()
        {
            var retiros = _service.ObtenerTodos();
            return View(retiros);
        }

        public ActionResult Crear()
        {
            CargarPaquetes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Retiro retiro)
        {
            if (string.IsNullOrWhiteSpace(retiro.RecibidoPor))
            {
                ModelState.AddModelError(
                    "RecibidoPor",
                    "Escribe el nombre de la persona que recibe el paquete.");
            }

            if (retiro.PaqueteId <= 0)
            {
                ModelState.AddModelError(
                    "PaqueteId",
                    "Selecciona un paquete válido.");
            }

            if (!ModelState.IsValid)
            {
                CargarPaquetes();
                return View(retiro);
            }

            bool registrado = _service.Registrar(retiro);

            if (!registrado)
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo registrar el retiro. Verifica que el paquete esté Notificado y que no haya sido retirado.");

                CargarPaquetes();
                return View(retiro);
            }

            TempData["Mensaje"] = "Retiro registrado correctamente.";
            return RedirectToAction("Index");
        }

        private void CargarPaquetes()
        {
            var paquetes = _paqueteRepository.ObtenerTodos()
                .FindAll(p => p.Estado == "Notificado");

            ViewBag.Paquetes = new SelectList(
                paquetes,
                "PaqueteId",
                "Descripcion");
        }
    }
}