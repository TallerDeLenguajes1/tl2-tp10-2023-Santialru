using System;
using System.Collections.Generic;
using System.Data.SQLite;
using tl2_tp10_2023_Santialru.Models;

namespace tl2_tp10_2023_Santialru.repos
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/kanbanV2.db;Cache=Shared";

        public void CrearUsuario(Usuario usuario)
        {
         var query = @"INSERT INTO usuario (nombre_de_usuario,rol,contrasenia) VALUES (@nombre_de_usuario,@rol,@contrasenia)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));

                command.ExecuteNonQuery();

                connection.Close();   
            }
        }

        public void ModificarUsuario(int id, Usuario usuarioModificado)
        {
            var query = $"UPDATE Usuario SET nombre_de_usuario = @nombre_de_usuario,rol = @rol,contrasenia = @contrasenia  WHERE id=@idUsuario";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuarioModificado.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@rol", (int)usuarioModificado.Rol));
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuarioModificado.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuarioModificado.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@idUsuario", id));

                command.ExecuteNonQuery();

                connection.Close();   
            }
        }

        public List<Usuario> ListarUsuarios()
        {
            var query = @"SELECT * FROM Usuario;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                List<Usuario> usuarios = new List<Usuario>();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NombreDeUsuario = reader["nombre_de_usuario"].ToString()
                        };
                        usuarios.Add(usuario);
                    }
                }

                connection.Close();

                return usuarios;
            }
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            var query = "SELECT * FROM usuario WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NombreDeUsuario = reader["nombre_de_usuario"].ToString()
                        };
                        return usuario;
                    }
                }

                connection.Close();
            }

            return null;
        }

        public void EliminarUsuario(int id)
        {
            var query = $"DELETE FROM usuario WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

            public Usuario ObtenerUsuarioPorNombre(string nombre)
    {
        SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
        var usuario = new Usuario();
        SQLiteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM usuario WHERE nombre_de_usuario = @nombre";
        command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
        connection.Open();
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            // Si no hay filas, lanzar una excepción
            if (!reader.HasRows)
            {
                connection.Close();
                throw new Exception("No se encontró el usuario");
            }

            while (reader.Read())
            {
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                usuario.Contrasenia = reader["contrasenia"].ToString();
                usuario.Rol = Enum.Parse<Rol>(reader["rol"].ToString());
            }
        }
        connection.Close();

        return usuario;
    }
    }
}
