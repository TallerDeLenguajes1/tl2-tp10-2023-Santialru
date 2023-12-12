using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Threading.Tasks;
using tl2_tp10_2023_Santialru.Models;


namespace tl2_tp10_2023_Santialru.repos;

public class TableroRepository : ITableroRepository
{
    private string cadenaConexion = "Data Source=DB/kanbanV2.db;Cache=Shared";

    public void CrearTablero(Tablero tablero)
    {
        var query = $"INSERT INTO tablero (id, id_usuario_propietario, nombre, descripcion) VALUES (@id,@id_usuario_propietario, @nombre, @descripcion)";
        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            
            command.Parameters.Add(new SQLiteParameter("@id", tablero.Id));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_propietario", tablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    
    public void ModificarTablero(int id, Tablero tablero)
    {
        var query = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idPropietario WHERE id = @id;";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@idPropietario", tablero.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Tablero ObtenerTableroPorId(int idTablero)
    {
        SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
        var tablero = new Tablero();
        SQLiteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM tablero WHERE id = '{idTablero}';";
        command.Parameters.Add(new SQLiteParameter("@id", idTablero));
        connection.Open();
        using(SQLiteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                tablero.Id = Convert.ToInt32(reader["id"]);
                tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                tablero.Nombre = reader["nombre"].ToString();
                tablero.Descripcion = reader["descripcion"].ToString();
            }
        }
        connection.Close();

        return tablero;
    }

    public List<Tablero> ListarTableros()
    {
        var queryString = $"SELECT * FROM tablero;";
        List<Tablero> tableros = new List<Tablero>();
        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return tableros;
    }
    
    public List<Tablero> ListarTablerosDeUsuario(int idUsuario)
    {
        var tableros = new List<Tablero>();
        var query = "SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var tablero = new Tablero
                {
                    Id = Convert.ToInt32(reader["id"]),
                    IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                    Nombre = reader["nombre"].ToString(),
                    Descripcion = reader["descripcion"].ToString()
                };
                tableros.Add(tablero);
            }

            connection.Close();
        }

        return tableros;
    }

    public void EliminarTablero(int id)
    {
        var query = "DELETE FROM Tablero WHERE id = @id";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

        public Tablero ObtenerTableroPorNombre(string nombre)
        {
            var query = "SELECT * FROM Tablero WHERE nombre = @nombre";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var tablero = new Tablero();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }
                connection.Close();
                    // Lanzar una excepción si no se encontró ningún tablero
                if (tablero.Id == 0)
                {
                    throw new Exception("Tablero no encontrado");
                }
                return tablero;
            }
        }
}

