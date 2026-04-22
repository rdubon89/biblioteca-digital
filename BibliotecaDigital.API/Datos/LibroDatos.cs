using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Datos
{
    
    /// Capa de acceso a datos para la entidad Libro.
    /// Aquí se ejecutan los procedimientos almacenados relacionados
    /// con el catálogo de libros del sistema.
   
    public class LibroDatos
    {
        
        /// Obtiene todos los libros registrados en la base de datos.
        /// Incluye la información general del libro y el nombre de su categoría.
        
        /// <returns>Lista de libros.</returns>
        public List<Libro> ObtenerTodos()
        {
            List<Libro> lista = new List<Libro>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Libro_ObtenerTodos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Libro libro = new Libro
                    {
                        // Identificador único del libro
                        IdLibro = Convert.ToInt32(dr["IdLibro"]),

                        // Datos principales del libro
                        Titulo = dr["Titulo"].ToString(),
                        Autor = dr["Autor"].ToString(),

                        // ISBN puede venir nulo en BD
                        ISBN = dr["ISBN"] == DBNull.Value ? "" : dr["ISBN"].ToString(),

                        // Fecha de publicación puede ser nula
                        FechaPublicacion = dr["FechaPublicacion"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(dr["FechaPublicacion"]),

                        // Ruta del archivo físico puede venir nula
                        RutaArchivo = dr["RutaArchivo"] == DBNull.Value ? "" : dr["RutaArchivo"].ToString(),

                        // Nombre de la categoría
                        Categoria = dr["Categoria"].ToString(),

                        // Id de categoría:
                        // Si el procedimiento no lo devuelve todavía, se maneja de forma segura.
                        IdCategoria = dr["IdCategoria"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(dr["IdCategoria"])
                    };

                    lista.Add(libro);
                }
            }

            return lista;
        }

        
        /// Obtiene un libro específico según su Id.
        /// Devuelve toda la información necesaria para visualizarlo o editarlo.
        /// <param name="idLibro">Id del libro a buscar.</param>
        /// <returns>Objeto Libro si existe; de lo contrario, null.</returns>
        public Libro ObtenerPorId(int idLibro)
        {
            Libro libro = null;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Libro_ObtenerPorId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    libro = new Libro
                    {
                        // Identificador del libro
                        IdLibro = Convert.ToInt32(dr["IdLibro"]),

                        // Datos principales
                        Titulo = dr["Titulo"].ToString(),
                        Autor = dr["Autor"].ToString(),

                        // ISBN opcional
                        ISBN = dr["ISBN"] == DBNull.Value ? "" : dr["ISBN"].ToString(),

                        // Fecha de publicación opcional
                        FechaPublicacion = dr["FechaPublicacion"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(dr["FechaPublicacion"]),

                        // Ruta física o lógica del archivo
                        RutaArchivo = dr["RutaArchivo"] == DBNull.Value ? "" : dr["RutaArchivo"].ToString(),

                        // Id y nombre de categoría
                        IdCategoria = dr["IdCategoria"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(dr["IdCategoria"]),

                        Categoria = dr["Categoria"].ToString()
                    };
                }
            }

            return libro;
        }

        
        /// Inserta un nuevo libro en la base de datos.
      
        /// <param name="titulo">Título del libro.</param>
        /// <param name="autor">Autor del libro.</param>
        /// <param name="isbn">ISBN del libro.</param>
        /// <param name="fechaPublicacion">Fecha de publicación.</param>
        /// <param name="rutaArchivo">Ruta física del archivo almacenado.</param>
        /// <param name="idCategoria">Id de la categoría a la que pertenece.</param>
        /// <returns>Id del libro insertado.</returns>
        public int InsertarLibro(string titulo, string autor, string isbn, DateTime? fechaPublicacion, string rutaArchivo, int idCategoria)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Libro_Insertar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Autor", autor);

                // Si el ISBN está vacío, se guarda como NULL
                cmd.Parameters.AddWithValue("@ISBN",
                    string.IsNullOrWhiteSpace(isbn) ? (object)DBNull.Value : isbn);

                // Si no se ingresó fecha, se guarda como NULL
                cmd.Parameters.AddWithValue("@FechaPublicacion",
                    fechaPublicacion.HasValue ? (object)fechaPublicacion.Value : DBNull.Value);

                // Si no existe ruta, se guarda como NULL
                cmd.Parameters.AddWithValue("@RutaArchivo",
                    string.IsNullOrWhiteSpace(rutaArchivo) ? (object)DBNull.Value : rutaArchivo);

                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

       
        /// Actualiza la información de un libro existente.
        /// Permite modificar sus datos y, si aplica, la ruta del archivo.
        /// <param name="idLibro">Id del libro a actualizar.</param>
        /// <param name="titulo">Nuevo título del libro.</param>
        /// <param name="autor">Nuevo autor del libro.</param>
        /// <param name="isbn">Nuevo ISBN.</param>
        /// <param name="fechaPublicacion">Nueva fecha de publicación.</param>
        /// <param name="rutaArchivo">Nueva ruta del archivo o la actual si no cambia.</param>
        /// <param name="idCategoria">Nueva categoría del libro.</param>
        public void ActualizarLibro(int idLibro, string titulo, string autor, string isbn, DateTime? fechaPublicacion, string rutaArchivo, int idCategoria)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Libro_Actualizar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Autor", autor);

                // Si el ISBN está vacío, se guarda como NULL
                cmd.Parameters.AddWithValue("@ISBN",
                    string.IsNullOrWhiteSpace(isbn) ? (object)DBNull.Value : isbn);

                // Si la fecha está vacía, se guarda como NULL
                cmd.Parameters.AddWithValue("@FechaPublicacion",
                    fechaPublicacion.HasValue ? (object)fechaPublicacion.Value : DBNull.Value);

                // Si la ruta está vacía, se guarda como NULL
                cmd.Parameters.AddWithValue("@RutaArchivo",
                    string.IsNullOrWhiteSpace(rutaArchivo) ? (object)DBNull.Value : rutaArchivo);

                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        /// Elimina un libro de la base de datos según su Id.
        /// Este método elimina únicamente el registro lógico en BD.
        /// La eliminación del archivo físico debe manejarse desde la capa de presentación o negocio.
        
        /// <param name="idLibro">Id del libro a eliminar.</param>
        public void EliminarLibro(int idLibro)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Libro_Eliminar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}