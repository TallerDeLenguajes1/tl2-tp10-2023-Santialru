using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tp11.ViewModels;

namespace tp9.Models;
public enum NivelDeAcceso
{
    admin = 1,
    operador = 2
}
public class Usuario
{
    public NivelDeAcceso NivelDeAcceso {get;set;}
    public int Id {get;set;}
    public string NombreDeUsuario {get;set;}

    public Usuario(LoginViewModel LoginViewModel)
    {
        NombreDeUsuario = LoginViewModel.NombreDeUsuario;
        Contrasenia = LoginViewModel.Contrasenia;
    }

    public Usuario()
    {
    }

}