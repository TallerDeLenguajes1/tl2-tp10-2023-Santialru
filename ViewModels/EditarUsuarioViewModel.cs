using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_Santialru.Models;

namespace tl2_tp10_2023_Santialru.ViewModels
{
    public class EditarUsuarioViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario")] 
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre de Usuario")] 
        public string NombreDeUsuario { get; set; }
        

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Rol usuario")] 
        public Rol rol { get; set; }

        public EditarUsuarioViewModel(int id,string nombre,string contrasenia,Rol rol){
            this.Id = id;
            this.NombreDeUsuario = nombre;
            this.Contrasenia = contrasenia;
            this.rol = rol;
        }

        public EditarUsuarioViewModel(){
            this.Id = 0;
            this.NombreDeUsuario = "";
            this.Contrasenia = "";
            this.rol = 0;
        }
    }
}