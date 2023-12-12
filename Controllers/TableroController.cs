using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Logging;
using tl2_tp10_2023_Santialru.Models;
using tl2_tp10_2023_Santialru.repos;
using tl2_tp10_2023_Santialru.ViewModels;

namespace tl2_tp10_2023_Santialru.Controllers
{
    [Route("[controller]")]
    public class TableroController : Controller
    {
        private readonly ILogger<TableroController> _logger;
        private ITableroRepository _repositorioTablero;
        private IUsuarioRepository _repositorioUsuario;

        public TableroController(ILogger<TableroController> logger, ITableroRepository repositorioTablero, IUsuarioRepository repositorioUsuario)
        {
            _logger = logger;
            _repositorioTablero=repositorioTablero;
            _repositorioUsuario=repositorioUsuario; 
        }
    

        [Route("index")]
        public IActionResult Index()
        {
            if(HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario")!=null){
                string rolUsuario = ObtenerRolUsuario();
                if (rolUsuario == "administrador")
                {
                    return View(ListarTableroViewModel());
                }
                else
                {
                    var idUsuario = HttpContext.Session.GetInt32("Id");
                    return View(ListarTableroPorUsuarioViewModel(idUsuario.Value));
                    
                }
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
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
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){

                var tablero = _repositorioTablero.ObtenerTableroPorId(id);
                var tablerovm = new EditarTableroViewModel(tablero);
                var usuario = _repositorioUsuario.ObtenerUsuarioPorId(tablero.IdUsuarioPropietario);
                tablerovm.NombreUsuario = usuario.NombreDeUsuario;
                return View(tablerovm);
            }    
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
        }

        [HttpPost]
        [Route("EditarTablero")] //  dataBaseLocked
        public IActionResult EditarTablero(EditarTableroViewModel tablero)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                try
                {
                    var tableroMod = _repositorioTablero.ObtenerTableroPorId(tablero.Id);
                    var usuarioPropietario = _repositorioUsuario.ObtenerUsuarioPorNombre(tablero.NombreUsuario);
                    if (usuarioPropietario.NombreDeUsuario != "")
                    {
                        tableroMod.IdUsuarioPropietario = usuarioPropietario.Id;
                    }else
                    {
                        tableroMod.IdUsuarioPropietario = tablero.Id_Usuario_Propietario;  
                    }
                    tableroMod.Nombre = tablero.Nombre;
                    tableroMod.Descripcion = tablero.Descripcion;
                    _repositorioTablero.ModificarTablero(tablero.Id,tableroMod);                    
                    }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }

                return RedirectToAction("Index");
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });

        }

        [HttpGet]
        [Route("CrearTablero")]
        public IActionResult CrearTablero()
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                return View(new CrearTableroViewModel());
            }   
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
        }

        [HttpPost]
        [Route("CrearTablero")]
        public IActionResult CrearTablero(CrearTableroViewModel t)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                var idUsuario = HttpContext.Session.GetInt32("Id");
                var tablero = new Tablero(0,idUsuario.Value,t.Nombre,t.Descripcion);
                try
                {
                    _repositorioTablero.CrearTablero(tablero);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                return RedirectToAction("Index");
            }
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

        }

        [HttpPost]
        [Route("EliminarTablero")]
        public IActionResult EliminarTablero(int id)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.GetString("Usuario") != null){
                try
                {
                    _repositorioTablero.EliminarTablero(id);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
              
                return RedirectToAction("Index");
            }    
            return (RedirectToRoute(new { Controller = "Home", action = "Index" }));

        }
        
        private string ObtenerRolUsuario()
        {
            // Verifica si existe la sesión y si contiene el rol del usuario.
            if (HttpContext.Session.TryGetValue("Rol", out var rolBytes))
            {
                // Convierte los bytes almacenados en la sesión de nuevo a una cadena.
                var rol = Encoding.UTF8.GetString(rolBytes);
                return rol;
            }

            // Si no se encuentra el rol en la sesión, retorna una cadena vacía o nula.
            return string.Empty;
        }

        private List<IndexTableroViewModel> ListarTableroViewModel(){
            var ListaTablerosViewModel = new List<IndexTableroViewModel>();
            var listaTablero = _repositorioTablero.ListarTableros();
            foreach (var item in listaTablero)
            {
                Usuario? Usu = _repositorioUsuario.ObtenerUsuarioPorId(item.IdUsuarioPropietario);
                string? nombre;
                if (Usu != null)
                {
                    nombre = Usu.NombreDeUsuario;
                }
                else
                {
                    nombre = null;
                }
                var TableroVM = new IndexTableroViewModel(item, nombre);
                ListaTablerosViewModel.Add(TableroVM);
            }
            return ListaTablerosViewModel;
        }

        private List<IndexTableroViewModel> ListarTableroPorUsuarioViewModel(int id){
            var ListaTablerosViewModel = new List<IndexTableroViewModel>();
            var listaTablero = _repositorioTablero.ListarTablerosDeUsuario(id);
            foreach (var item in listaTablero)
            {
                Usuario? Usu = _repositorioUsuario.ObtenerUsuarioPorId(item.IdUsuarioPropietario);
                string? nombre;
                if (Usu != null)
                {
                    nombre = Usu.NombreDeUsuario;
                }
                else
                {
                    nombre = null;
                }
                var TableroVM = new IndexTableroViewModel(item, nombre);
                ListaTablerosViewModel.Add(TableroVM);
            }
            return ListaTablerosViewModel;
        }
    }
}
