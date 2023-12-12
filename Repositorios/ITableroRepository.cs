using System.Collections.Generic;
using tl2_tp10_2023_Santialru.Models;
namespace tl2_tp10_2023_Santialru.repos;
public interface ITableroRepository
{
    public void CrearTablero(Tablero tablero);
    public void ModificarTablero(int id, Tablero tablero);
    public Tablero ObtenerTableroPorId(int id);
    public List<Tablero> ListarTableros();
    public List<Tablero> ListarTablerosDeUsuario(int idUsuario);
    public void EliminarTablero(int id);
    Tablero ObtenerTableroPorNombre(string nombre);

}
