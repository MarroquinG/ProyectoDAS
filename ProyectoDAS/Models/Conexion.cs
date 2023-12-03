using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProyectoDAS.Models
{
    public class Conexion
    {
        private string cadenaConexion { get; set; }
        private SqlConnection conexionSQL;

        public Conexion()
        {
            cadenaConexion = @"Data source=(local);Initial Catalog=AgenciaDeViaje;Integrated Security=True";
        }

        public bool Conectar()
        {
            try
            {
                conexionSQL = new SqlConnection(cadenaConexion);
                conexionSQL.Open();
                Debug.WriteLine("Conexión establecida correctamente.");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Conexión establecida NO correctamente.");
                return false;
            }
        }

        public bool EstadoConexion()
        {
            switch (conexionSQL.State)
            {
                case System.Data.ConnectionState.Broken:
                case System.Data.ConnectionState.Open:
                    return true;
                default:
                    return false;
            }
        }

        public void Desconectar()
        {
            conexionSQL.Close();
        }

        public List<Destinos> ListarDestinos(string nombre, string pais)
        {
            string SQL = "SELECT * FROM Destinos WHERE Nombre LIKE @Nombre AND Pais LIKE @Pais";
            DataTable t = new DataTable();

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
            comando.Parameters.AddWithValue("@Pais", "%" + pais + "%");

            SqlDataAdapter dataAdaptador = new SqlDataAdapter(comando);
            dataAdaptador.Fill(t);

            List<Destinos> destinos = new List<Destinos>();

            foreach (DataRow fila in t.Rows)
            {
                destinos.Add(new Destinos
                {
                    DestinoID = Convert.ToInt32(fila["DestinoID"]),
                    Nombre = Convert.ToString(fila["Nombre"]),
                    ImagenURL = Convert.ToString(fila["ImagenURL"]),
                    Pais = Convert.ToString(fila["Pais"]),
                    Requisitos = Convert.ToString(fila["Requisitos"])
                });
            }

            return destinos;
        }

        public Destinos ObtenerDestinoPorID(int destinoID)
        {
            string SQL = "SELECT * FROM Destinos WHERE DestinoID = @DestinoID";
            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@DestinoID", destinoID);

            SqlDataReader reader = comando.ExecuteReader();

            Destinos destino = null;

            if (reader.Read())
            {
                destino = new Destinos
                {
                    DestinoID = Convert.ToInt32(reader["DestinoID"]),
                    Nombre = Convert.ToString(reader["Nombre"]),
                    ImagenURL = Convert.ToString(reader["ImagenURL"]),
                    Pais = Convert.ToString(reader["Pais"]),
                    Requisitos = Convert.ToString(reader["Requisitos"])
                };
            }

            reader.Close();

            return destino;
        }

        public List<Actividades> ObtenerActividadesPorDestino(int destinoID)
        {
            string SQL = "SELECT * FROM Actividades WHERE DestinoID = @DestinoID";
            DataTable t = new DataTable();

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@DestinoID", destinoID);

            SqlDataAdapter dataAdaptador = new SqlDataAdapter(comando);
            dataAdaptador.Fill(t);

            List<Actividades> actividades = new List<Actividades>();

            foreach (DataRow fila in t.Rows)
            {
                actividades.Add(new Actividades
                {
                    ActividadID = Convert.ToInt32(fila["ActividadID"]),
                    Nombre = Convert.ToString(fila["Nombre"]),
                    TipoActividad = Convert.ToString(fila["TipoActividad"]),
                    Dias = Convert.ToInt32(fila["Dias"]),
                    Precio = Convert.ToDecimal(fila["Precio"])
                });
            }

            return actividades;
        }

        public void ReservarDestino(int usuarioID, int destinoID)
        {
            string SQL = "INSERT INTO Reservas (UsuarioID, DestinoID, FechaReserva) VALUES (@UsuarioID, @DestinoID, GETDATE())";

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.Parameters.AddWithValue("@DestinoID", destinoID);

            comando.ExecuteNonQuery();
        }

        public List<Destinos> ObtenerDestinosAleatorios()
        {
            string SQL = "SELECT TOP 3 * FROM Destinos ORDER BY NEWID()";
            DataTable t = new DataTable();

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            SqlDataAdapter dataAdaptador = new SqlDataAdapter(comando);
            dataAdaptador.Fill(t);

            List<Destinos> destinos = new List<Destinos>();

            foreach (DataRow fila in t.Rows)
            {
                destinos.Add(new Destinos
                {
                    DestinoID = Convert.ToInt32(fila["DestinoID"]),
                    Nombre = Convert.ToString(fila["Nombre"]),
                    ImagenURL = Convert.ToString(fila["ImagenURL"]),
                    Pais = Convert.ToString(fila["Pais"]),
                    Requisitos = Convert.ToString(fila["Requisitos"])
                });
            }

            return destinos;
        }

        public List<DestinosBuscados> ObtenerBusquedasRecientes()
        {
            string SQL = "SELECT * FROM DestinosBuscados WHERE UsuarioID = @UsuarioID ORDER BY FechaBusqueda DESC";
            DataTable t = new DataTable();

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@UsuarioID", 1);

            SqlDataAdapter dataAdaptador = new SqlDataAdapter(comando);
            dataAdaptador.Fill(t);

            List<DestinosBuscados> busquedasRecientes = new List<DestinosBuscados>();

            foreach (DataRow fila in t.Rows)
            {
                int idUsuario = Convert.ToInt32(fila["UsuarioID"]);
                int idDestino = Convert.ToInt32(fila["DestinoID"]);
                string NombreUsuario = ObtenerNombreUsuario(idUsuario);
                string NombrePais = ObtenerNombrePais(idDestino);
                string NombreImagen = ObtenerNombreImagen(idDestino);

                busquedasRecientes.Add(new DestinosBuscados
                {
                    DestinoBuscadoID = Convert.ToInt32(fila["DestinoBuscadoID"]),
                    UsuarioNombre = NombreUsuario,
                    DestinoNombre = NombrePais,
                    imagen = NombreImagen,
                    FechaBusqueda = Convert.ToDateTime(fila["FechaBusqueda"])

                
            });
            }

            return busquedasRecientes;
        }

        public void GuardarBusquedaReciente(int usuarioID, int destinoID)
        {
            string SQL = "INSERT INTO DestinosBuscados (UsuarioID, DestinoID, FechaBusqueda) VALUES (@UsuarioID, @DestinoID, GETDATE())";

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.Parameters.AddWithValue("@DestinoID", destinoID);

            comando.ExecuteNonQuery();
        }

        public List<Reservas> ObtenerReservasPorUsuario()
        {
            string SQL = "SELECT * FROM Reservas";
            DataTable t = new DataTable();

            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
          

            SqlDataAdapter dataAdaptador = new SqlDataAdapter(comando);
            dataAdaptador.Fill(t);

            List<Reservas> reservas = new List<Reservas>();

            foreach (DataRow fila in t.Rows)
            {
                int idUsuario = Convert.ToInt32(fila["UsuarioID"]);
                int idDestino = Convert.ToInt32(fila["DestinoID"]);
                string NombreUsuario = ObtenerNombreUsuario(idUsuario);
                string NombrePais = ObtenerNombrePais(idDestino);
                string NombreImagen = ObtenerNombreImagen(idDestino);
                reservas.Add(new Reservas
                {
                    ReservaID = Convert.ToInt32(fila["ReservaID"]),
                    UsuarioNombre = NombreUsuario,
                    DestinoNombre = NombrePais,
                    Imagen = NombreImagen,
                    FechaReserva = Convert.ToDateTime(fila["FechaReserva"])
                });
            }

            return reservas;
        }

        private string ObtenerNombreUsuario(int idUsuario)
        {
            string SQL = "SELECT NombreUsuario FROM Usuarios  WHERE UsuarioID = @UsuarioID";
            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@UsuarioID", idUsuario);

            object result = comando.ExecuteScalar();

            return result != null ? result.ToString() : string.Empty;
        }


        private string ObtenerNombrePais(int idDestino)
        {
            string SQL = "SELECT pais FROM Destinos WHERE DestinoID = @DestinoID";
            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@DestinoID", idDestino);

            object result = comando.ExecuteScalar();

            return result != null ? result.ToString() : string.Empty;
        }

        private string ObtenerNombreImagen(int idDestino)
        {
            string SQL = "SELECT ImagenURL FROM Destinos WHERE DestinoID = @DestinoID";
            SqlCommand comando = new SqlCommand(SQL, conexionSQL);
            comando.Parameters.AddWithValue("@DestinoID", idDestino);

            object result = comando.ExecuteScalar();

            return result != null ? result.ToString() : string.Empty;
        }


    }
}
