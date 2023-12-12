namespace tl2_tp10_2023_Santialru.Models;
public class Tarea
{
    public Tarea()
    {
    }

        public Tarea(int id, int idTablero, string nombre, EstadoTarea estado, string? descripcion, string? color, int idUsuarioAsignado)
        {
            Id = id;
            IdTablero = idTablero;
            Nombre = nombre;
            Estado = estado;
            Descripcion = descripcion;
            Color = color;
            IdUsuarioPropietario = idUsuarioAsignado;
        }

    public enum EstadoTarea
    {
        Ideas,
        ToDo,
        Doing,
        Review,
        Done
    }


    public int Id{get;set;}
    public int IdTablero{get;set;}
    public string Nombre{get;set;}
    public string? Descripcion{get;set;}
    public string? Color{get;set;}
    public EstadoTarea Estado{get;set;}
    public int IdUsuarioPropietario{get;set;}

  
}

