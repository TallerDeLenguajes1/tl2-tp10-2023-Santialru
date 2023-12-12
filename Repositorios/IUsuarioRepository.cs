
using System.Collections.Generic;
using tl2_tp10_2023_Santialru.Models;

namespace tl2_tp10_2023_Santialru.repos;
public interface IUsuarioRepository
{
    void CrearUsuario(Usuario usuario);
    void ModificarUsuario(int id, Usuario usuario);
    List<Usuario> ListarUsuarios();
    Usuario ObtenerUsuarioPorNombre(string nombre);

    Usuario ObtenerUsuarioPorId(int id);

    void EliminarUsuario(int id);
}
