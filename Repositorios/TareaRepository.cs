using System;
using System.Collections.Generic;
using System.Data.SQLite;
using tl2_tp10_2023_Santialru.Models;

namespace tl2_tp10_2023_Santialru.repos
{
    public class TareaRepository :ITareaRepository
    {
        private string cadenaConexion = "Data Source=DB/kanbanV2.db;Cache=Shared";

        public Tarea CrearTarea(int idTablero, Tarea tarea)
        {
            var query = @"INSERT INTO Tarea (Id_Tablero, Nombre, Estado, Descripcion, Color, Id_Usuario_Propietario) 
                    VALUES (@idTablero, @nombre, @estado, @descripcion, @color, @idUsuarioPropietario)";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", tarea.Nombre);
                command.Parameters.AddWithValue("@descripcion", tarea.Descripcion);
                command.Parameters.AddWithValue("@color", tarea.Color);
                command.Parameters.AddWithValue("@estado", tarea.Estado.ToString());
                command.Parameters.AddWithValue("@idUsuarioPropietario", tarea.IdUsuarioPropietario);
                command.Parameters.AddWithValue("@idTablero", idTablero);

                int tareaId = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                tarea.Id = tareaId;
                return tarea;
            }
        }

        public void ModificarTarea(int id, Tarea tarea)
        {
            var query = @"UPDATE Tarea 
                        SET Id_Tablero = @idTablero,
                            Nombre = @nombre,
                            Estado = @estado,
                            Descripcion = @descripcion,
                            Color = @color,
                            Id_Usuario_Propietario = @idUsuarioPropietario
                        WHERE Id = @id";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.IdTablero));
                command.Parameters.AddWithValue("@nombre", tarea.Nombre);
                command.Parameters.AddWithValue("@descripcion", tarea.Descripcion);
                command.Parameters.AddWithValue("@color", tarea.Color);
                command.Parameters.AddWithValue("@estado", tarea.Estado.ToString());
                command.Parameters.AddWithValue("@idUsuarioPropietario", tarea.IdUsuarioPropietario);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public Tarea ObtenerTareaPorId(int id)
        {
            var tarea = new Tarea();
            var query = "SELECT * FROM Tarea WHERE Id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_Tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = Enum.Parse<Tarea.EstadoTarea>(reader["estado"].ToString());
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioPropietario = Convert.ToInt32(reader["id_Usuario_Propietario"]);
                }
                connection.Clone();
            }

            return tarea;
        }

        public List<Tarea> ListarTareasDeUsuario(int idUsuario)
        {
            List<Tarea> tareas = new List<Tarea>();
            var query = "SELECT * FROM tarea WHERE id_usuario_propietario = @idUsuario;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                    Id = Convert.ToInt32(reader["Id"]),
                    IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                    Nombre = reader["Nombre"].ToString(),
                    Estado = Enum.Parse<Tarea.EstadoTarea>(reader["Estado"].ToString()),
                    Descripcion = reader["Descripcion"].ToString(),
                    Color = reader["color"].ToString(),
                    IdUsuarioPropietario = Convert.ToInt32(reader["id_Usuario_Propietario"])
                    };
                    tareas.Add(tarea);
                }
                connection.Close();
            }
            return tareas;
        }

        public List<Tarea> ListarTareasDeTablero(int idTablero)
        {
            var query = "SELECT * FROM tarea WHERE id_tablero = @idTablero;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@idTablero", idTablero);

                List<Tarea> tareas = new List<Tarea>();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            Color = reader["color"].ToString(),
                            Estado = Enum.Parse<Tarea.EstadoTarea>(reader["estado"].ToString()),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                        };
                        tareas.Add(tarea);
                    }
                }

                connection.Close();

                return tareas;
            }
        }

        public void EliminarTarea(int idTarea)
        {
            var query = "DELETE FROM tarea WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", idTarea);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            var query = "UPDATE tarea SET id_usuario_propietario = @idUsuario WHERE id = @idTarea;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@idTarea", idTarea);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void ActualizarTareaPorNombre(int id, string nuevoNombre)
        {
            var query = "UPDATE tarea SET nombre = @nuevoNombre WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void ActualizarTareaPorEstado(int id, int nuevoEstado)
        {
            var query = "UPDATE tarea SET estado = @nuevoEstado WHERE id = @id;";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@nuevoEstado", nuevoEstado.ToString());
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public int GetCantidadTareasPorEstado(int estado)
        {
            var query = "SELECT COUNT(*) as cantidad FROM tarea WHERE estado = @estado;";
            int cantidadTareas = 0;

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@estado", estado);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cantidadTareas = Convert.ToInt32(reader["cantidad"]);
                    }
                }

                connection.Close();
            }

            return cantidadTareas;
        }




        public List<Tarea> ListarTareasPorTableros(List<int> idsTableros)
        {
            var tareas = new List<Tarea>();

            if (idsTableros == null || !idsTableros.Any())
            {
                throw new ArgumentException("La lista de IDs de tableros no puede ser nula o vacía");
            }

            // Construir la cadena IN dinámicamente
            var inClause = string.Join(",", idsTableros);

            // Consulta SQL con la cadena IN construida
            var query = $"SELECT * FROM Tarea WHERE Id_Tablero IN ({inClause})";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Si no hay filas, lanzar una excepción
                    if (!reader.HasRows)
                    {
                        connection.Close();
                        throw new Exception("No se encontraron tareas para los tableros especificados");
                    }

                    while (reader.Read())
                    {
                        var tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                            Nombre = reader["Nombre"].ToString(),
                            Estado = Enum.Parse<Tarea.EstadoTarea>(reader["Estado"].ToString()),
                            Descripcion = reader["Descripcion"].ToString(),
                            Color = reader["Color"].ToString(),
                            IdUsuarioPropietario = Convert.ToInt32(reader["Id_Usuario_Propietario"])
                        };
                        tareas.Add(tarea);
                    }
                }
            }

            return tareas;
        }

    }
}
