namespace tl2_tp10_2023_Santialru.Models;
public class Tablero
{
    public Tablero()
    {
    }

    public int Id{get;set;}
    public int IdUsuarioPropietario{get;set;}
    public string Nombre{get;set;}
    public string Descripcion{get;set;}

    public Tablero(int id,int idUsuario,string nombre,string? descripcion){
        this.Id = id;
        this.IdUsuarioPropietario = idUsuario;
        this.Nombre = nombre;
        this.Descripcion = descripcion;

    }
    
}