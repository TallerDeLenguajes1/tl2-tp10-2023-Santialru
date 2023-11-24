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
    public class TableroController : Controller
    {
        private readonly ILogger<TableroController> _logger;
        private TableroRepository manejo;

        public TableroController(ILogger<TableroController> logger)
        {
            _logger = logger;
            manejo = new TableroRepository(); 
        }
    

        [Route("index")]
        public IActionResult Index()
        {
            var tableros = manejo.ListarTableros();
            return View(tableros);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        [HttpGet]
        [Route("EditarTablero")]
        public IActionResult EditarTablero(int id)
        {
            var tablero = manejo.ObtenerTableroPorId(id);
            return View(tablero);
        }

        [HttpPost]
        [Route("EditarTablero")] //  dataBaseLocked
        public IActionResult EditarTablero(Tablero tablero)
        {
            var tableroMod = manejo.ObtenerTableroPorId(tablero.Id1);
            tableroMod.Nombre1 = tablero.Nombre1;
            tableroMod.Descripcion1 = tablero.Descripcion1;
            tableroMod.IdUsuarioPropietario1 = tablero.IdUsuarioPropietario1;
            manejo.ModificarTablero(tablero.Id1,tableroMod);
            return RedirectToAction("Index");

        }

                [HttpGet]
        [Route("CrearTablero")]
        public IActionResult CrearTablero()
        {
            return View(new Tablero());
        }

        [HttpPost]
        [Route("CrearTablero")]
        public IActionResult CrearTablero(Tablero u)
        {
            manejo.CrearTablero(u);
            return RedirectToAction("Index");

        }

        [HttpPost]
        [Route("EliminarTablero")]
        public IActionResult EliminarTablero(int id)
        {
            manejo.EliminarTablero(id);
            return RedirectToAction("Index");

        }
        
    }
}
