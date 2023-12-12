using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp10_2023_Santialru.Models;
using tl2_tp10_2023_Santialru.repos;
using tl2_tp10_2023_Santialru.ViewModels;

namespace tl2_tp10_2023_Santialru.controllers
{
        [Route("[controller]")]
    public class TareaController : Controller
    {
        private readonly ITareaRepository _repositorioTarea;
        private readonly ITableroRepository _repositorioTablero;
        private IUsuarioRepository _repositorioUsuario;
        private readonly ILogger<TareaController> _logger;

        public TareaController(ILogger<TareaController> logger, ITareaRepository repositorioTarea, ITableroRepository repositorioTablero, IUsuarioRepository repositorioUsuario)
        {
            _logger = logger;
            _repositorioTablero =repositorioTablero;
            _repositorioTarea = repositorioTarea;
            _repositorioUsuario = repositorioUsuario;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario")!=null)
            {
                return View(ListarTareasIndex());
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index"});
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
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario")!= null)
            {
                var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                var tareaVm = new EditarTareaViewModel();
                tareaVm.Id = tarea.Id;
                tareaVm.Descripcion = tarea.Descripcion;
                tareaVm.Color = tarea.Color;
                tareaVm.EstadoT = tarea.Estado;
                tareaVm.IdTablero = tarea.IdTablero;
                tareaVm.IdUsuarioAsignado = tarea.IdUsuarioPropietario;
                tareaVm.Nombre = tarea.Nombre;
                var usu = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioPropietario);
                tareaVm.NombreUsuarioAsignado= usu.NombreDeUsuario;
                var tab = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);
                tareaVm.NombreTablero = tab.Nombre;
                return View(tareaVm);
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
        }

        [HttpPost]
        [Route("EditarTarea")]
        public IActionResult EditarTarea(int id,EditarTareaViewModel modificada)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                try
                {
                    var tarea = _repositorioTarea.ObtenerTareaPorId(id);
                    var usuario = _repositorioUsuario.ObtenerUsuarioPorNombre(modificada.NombreUsuarioAsignado);
                    var tablero = _repositorioTablero.ObtenerTableroPorNombre(modificada.NombreTablero);
                    int usuarioAsignado; // creo esta variable ya que en el constructor de tarea no puedo manda modificada.IdUsuarioAsignado
                    if (usuario != null)
                    {
                        usuarioAsignado = usuario.Id;
                    }else
                    {
                        usuarioAsignado=tarea.IdUsuarioPropietario;
                    }
                    if (tablero != null)
                    {
                        modificada.IdTablero = tablero.Id;
                    }else
                    {
                        modificada.IdTablero = tarea.IdTablero;
                    }
                    _repositorioTarea.ModificarTarea(id, new Tarea(
                    modificada.Id,
                    modificada.IdTablero,
                    modificada.Nombre,
                    modificada.EstadoT,
                    modificada.Descripcion,
                    modificada.Color,
                    usuarioAsignado));
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.ToString());
                    
                }
                return RedirectToAction("Index");
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });

        }

        [HttpGet]
        [Route("CrearTarea")]
        public IActionResult CrearTarea()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                return View(new CrearTareaViewModel());
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
        }

        [HttpPost]
        [Route("CrearTarea")]
        public IActionResult CrearTarea(CrearTareaViewModel nueva)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                try
                {
                    var tarea = new Tarea();
                    tarea.Id = nueva.Id;
                    tarea.Color = nueva.Color;
                    tarea.Descripcion = nueva.Descripcion;
                    tarea.Nombre = nueva.Nombre;
                    tarea.Estado = nueva.EstadoT;
                    var usu = _repositorioUsuario.ObtenerUsuarioPorNombre(nueva.NombreUsuarioAsignado);
                    tarea.IdUsuarioPropietario = usu.Id;
                    var tab = _repositorioTablero.ObtenerTableroPorNombre(nueva.NombreTablero);
                    _repositorioTarea.CrearTarea(tab.Id,tarea);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                return RedirectToAction("Index");
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });

        }

        [HttpPost]
        [Route("EliminarTarea")]
        public IActionResult EliminarTarea(int id)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                try
                {
                    _repositorioTarea.EliminarTarea(id);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                
                return RedirectToAction("Index");
            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

        }


            private List<IndexTareaViewModel> ListarTareasIndex()
            {
                var listaTareasIndex = new List<IndexTareaViewModel>();

                // Obtener el id del usuario de sesión
                var idUsuarioSesion = ObtenerIdUsuarioSesion();

                // Obtener las tareas asignadas al usuario de sesión
                var tareasAsignadas = _repositorioTarea.ListarTareasDeUsuario(idUsuarioSesion);

                // Obtener los tableros del usuario de sesión
                var tablerosUsuario = _repositorioTablero.ListarTablerosDeUsuario(idUsuarioSesion);

                // Obtener las tareas de los tableros del usuario de sesión
                var tareasPropias = _repositorioTarea.ListarTareasPorTableros(tablerosUsuario.Select(t => t.Id).ToList());

                // Combinar y eliminar duplicados
                var todasLasTareas = tareasAsignadas.Union(tareasPropias).Distinct().ToList();

                foreach (var tarea in todasLasTareas)
                {
                    var tablero = _repositorioTablero.ObtenerTableroPorId(tarea.IdTablero);
                    var usuarioAsignado = _repositorioUsuario.ObtenerUsuarioPorId(tarea.IdUsuarioPropietario);

                    string? nombreUsuarioAsignado = usuarioAsignado?.NombreDeUsuario;
                    string? nombreTablero = tablero?.Nombre;

                    var tarvm = new IndexTareaViewModel(tarea, nombreUsuarioAsignado, nombreTablero);
                    listaTareasIndex.Add(tarvm);
                }

                return listaTareasIndex;
            }

            // Método para obtener el id del usuario de sesión
          private int ObtenerIdUsuarioSesion()
            {
                return HttpContext.Session.IsAvailable ? HttpContext.Session.GetInt32("Id") ?? 0 : 0;
            }


    }
}