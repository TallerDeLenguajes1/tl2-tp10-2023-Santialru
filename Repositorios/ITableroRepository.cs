using System.Collections.Generic;
using tp9.Models;
namespace tp9.repos;
public interface ITableroRepository
{
    public void CrearTablero(Tablero tablero);
    public void ModificarTablero(int id, Tablero tablero);
    public Tablero ObtenerTableroPorId(int id);
    public List<Tablero> ListarTableros();
    public List<Tablero> ListarTablerosDeUsuario(int idUsuario);
    public void EliminarTablero(int id);
}
