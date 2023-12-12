using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp10_2023_Santialru.ViewModels;

namespace tl2_tp10_2023_Santialru.Models;
public enum Rol
{
    admin,
    operador
}
public class Usuario
{
    public Rol Rol {get;set;}
    public int Id {get;set;}
    public string NombreDeUsuario {get;set;}
    public string Contrasenia {get;set;}


    public Usuario(string nombre,string contrasenia,Rol Rol){
        this.Id = 0;
        this.NombreDeUsuario = nombre;
        this.Contrasenia = contrasenia;
        this.Rol = Rol;
    }
    public Usuario(){
        this.Id = 0;
        this.Contrasenia = "";
        this.NombreDeUsuario = "";
        this.Rol = 0;
    }

}