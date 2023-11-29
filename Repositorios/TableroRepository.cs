using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Threading.Tasks;
using tp9.Models;


namespace tp9.repos;

public class TableroRepository : ITableroRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void CrearTablero(Tablero tablero)
    {
        var query = $"INSERT INTO tablero (id, id_usuario_propietario, nombre, descripcion) VALUES (@id,@id_usuario_propietario,@descripcion)";
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
        SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
        SQLiteCommand command = connection.CreateCommand();
        command.CommandText =  $"UPDATE tablero SET id = '{tablero.Id}', id_usuario_propietario = '{tablero.IdUsuarioPropietario}', nombre = '{tablero.Nombre}', descripcion = '{tablero.Descripcion}';";
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
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

        return (tablero);
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
            command.Parameters.Add(new SQLiteParameter("@id", idUsuario));
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
}

