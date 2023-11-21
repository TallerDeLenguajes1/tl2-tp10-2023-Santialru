using System.Collections.Generic;
using tp9.Models;

namespace tp9.repos;
public interface ITareaRepository
{
    Tarea CrearTareaEnTablero(int idTablero, Tarea tarea);
    Tarea ModificarTarea(int id, Tarea tarea);
    Tarea ObtenerTareaPorId(int id);
    List<Tarea> ListarTareasAsignadasAUsuario(int idUsuario);
    List<Tarea> ListarTareasDeTablero(int idTablero);
    void EliminarTarea(int idTarea);
    void AsignarUsuarioATarea(int idUsuario, int idTarea);
}
