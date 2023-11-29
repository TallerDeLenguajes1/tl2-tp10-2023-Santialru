namespace tp9.Models;
public class Tarea
{
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
    public string Descripcion{get;set;}
    public string Color{get;set;}
    public EstadoTarea Estado{get;set;}
    public int IdUsuarioPropietario{get;set;}

  
}

