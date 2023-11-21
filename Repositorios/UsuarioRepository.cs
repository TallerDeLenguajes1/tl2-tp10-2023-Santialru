using System;
using System.Collections.Generic;
using System.Data.SQLite;
using tp9.Models;

namespace tp9.repos
{
    public class UsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public void CrearUsuario(Usuario usuario)
        {
            var query = "INSERT INTO usuario (nombre_de_usuario) VALUES (@nombreDeUsuario);";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@nombreDeUsuario", usuario.NombreDeUsuario);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void ModificarUsuario(int id, Usuario usuario)
        {
            var query = "UPDATE usuario SET nombre_de_usuario = @nombreDeUsuario WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@nombreDeUsuario", usuario.NombreDeUsuario);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Usuario> ListarUsuarios()
        {
            var query = "SELECT * FROM usuario;";

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
            var query = "DELETE FROM usuario WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
