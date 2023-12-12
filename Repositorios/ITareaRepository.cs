using System.Collections.Generic;
using tl2_tp10_2023_Santialru.Models;

namespace tl2_tp10_2023_Santialru.repos;
public interface ITareaRepository
{
    Tarea CrearTarea(int idTablero, Tarea tarea);
    void ModificarTarea(int id, Tarea tarea);
    Tarea ObtenerTareaPorId(int id);
    List<Tarea> ListarTareasDeUsuario(int idUsuario);
    List<Tarea> ListarTareasDeTablero(int idTablero);
    void EliminarTarea(int idTarea);
    void AsignarUsuarioATarea(int idUsuario, int idTarea);
    List<Tarea> ListarTareasPorTableros(List<int> idsTableros);
}
