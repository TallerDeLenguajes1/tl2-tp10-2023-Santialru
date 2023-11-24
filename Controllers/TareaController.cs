using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tp9.Models;
using tp9.repos;

namespace tp9.controllers
{
        [Route("[controller]")]
    public class TareaController : Controller
    {
        private readonly ILogger<TareaController> _logger;
        private TareaRepository manejo;

        public TareaController(ILogger<TareaController> logger)
        {
            _logger = logger;
            manejo = new TareaRepository();
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var tareas = manejo.ListarTareasDeUsuario(5);
            return View(tareas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        [HttpGet]
        [Route("EditarTarea")]
        public IActionResult EditarTarea(int id)
        {
            var tarea = manejo.ObtenerTareaPorId(id);
            return View(tarea);
        }

        [HttpPost]
        [Route("EditarTarea")]
        public IActionResult EditarTarea(int id,Tarea modificada)
        {
            manejo.ModificarTarea(id,modificada);
             return RedirectToAction("Index");

        }

        [HttpGet]
        [Route("CrearTarea")]
        public IActionResult CrearTarea()
        {
            return View(new Tarea());
        }

        [HttpPost]
        [Route("CrearTarea")]
        public IActionResult CrearTarea(Tarea nueva)
        {
            manejo.CrearTarea(2,nueva);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("EliminarTarea")]
        public IActionResult EliminarTarea(int id)
        {
            manejo.EliminarTarea(id);
            return RedirectToAction("Index");

        }
    }
}