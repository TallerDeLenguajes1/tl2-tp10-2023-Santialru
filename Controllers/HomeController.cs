using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_Santialru.Models;
using tl2_tp10_2023_Santialru.Repository;
using tl2_tp10_2023_Santialru.ViewModels;

namespace tl2_tp10_2023_Santialru.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILoginRepository _repositorioLogin;
    public HomeController(ILogger<HomeController> logger, ILoginRepository repositorioLogin)
    {
        _repositorioLogin = repositorioLogin;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Login(LoginViewModel usuario)
    {
        try
        {
            var usuarioLogeado = _repositorioLogin.ObtenerUsuario(usuario.NombreDeUsuario, usuario.Contrasenia);
            LogearUsuario(usuarioLogeado);
            _logger.LogInformation("Usuario " + usuarioLogeado.NombreDeUsuario + " Ingresó correctamente");
            return RedirectToRoute(new { controller = "Usuario", action = "Index" });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e.ToString());
            _logger.LogWarning("Usuario invalido - Nombre de usuario: " + usuario.NombreDeUsuario + "/Contraseña: " + usuario.Contrasenia);
        }
        return RedirectToAction("Index");
    }

    private void LogearUsuario(Usuario user)
    {
        HttpContext.Session.SetInt32("Id", user.Id);
        HttpContext.Session.SetString("Contrasenia", user.Contrasenia);
        HttpContext.Session.SetString("Rol", user.Rol.ToString());
        HttpContext.Session.SetString("Usuario", user.NombreDeUsuario);
    }

    public IActionResult CerrarSesion()
    {
        return RedirectToRoute(new { controller = "Home", action = "Index" });
        
    }
}
