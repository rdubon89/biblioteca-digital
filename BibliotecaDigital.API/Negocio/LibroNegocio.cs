using System;
using System.Collections.Generic;
using BibliotecaDigital.API.Datos;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Negocio
{
 
    /// Capa de lógica de negocio para la entidad Libro.
    /// Se encarga de servir como intermediaria entre la presentación
    /// y la capa de acceso a datos.
   
    public class LibroNegocio
    {
        // Instancia de la capa de datos para libros
        private LibroDatos libroDatos = new LibroDatos();

        
        /// Obtiene la lista completa de libros registrados en el sistema.
       
        /// <returns>Lista de libros.</returns>
        public List<Libro> ObtenerTodos()
        {
            return libroDatos.ObtenerTodos();
        }

        
        /// Obtiene un libro específico según su identificador.
       
        /// <param name="idLibro">Id del libro a consultar.</param>
        /// <returns>Objeto Libro si existe; en caso contrario, null.</returns>
        public Libro ObtenerPorId(int idLibro)
        {
            return libroDatos.ObtenerPorId(idLibro);
        }

        
        /// Inserta un nuevo libro en la base de datos.
       
        /// <param name="titulo">Título del libro.</param>
        /// <param name="autor">Autor del libro.</param>
        /// <param name="isbn">ISBN del libro.</param>
        /// <param name="fechaPublicacion">Fecha de publicación.</param>
        /// <param name="rutaArchivo">Ruta física del archivo almacenado.</param>
        /// <param name="idCategoria">Id de la categoría del libro.</param>
        /// <returns>Id del libro insertado.</returns>
        public int InsertarLibro(string titulo, string autor, string isbn, DateTime? fechaPublicacion, string rutaArchivo, int idCategoria)
        {
            return libroDatos.InsertarLibro(titulo, autor, isbn, fechaPublicacion, rutaArchivo, idCategoria);
        }

       
        /// Actualiza la información de un libro existente.
        
        /// <param name="idLibro">Id del libro a actualizar.</param>
        /// <param name="titulo">Nuevo título.</param>
        /// <param name="autor">Nuevo autor.</param>
        /// <param name="isbn">Nuevo ISBN.</param>
        /// <param name="fechaPublicacion">Nueva fecha de publicación.</param>
        /// <param name="rutaArchivo">Ruta del archivo actual o nueva ruta si fue reemplazado.</param>
        /// <param name="idCategoria">Nueva categoría.</param>
        public void ActualizarLibro(int idLibro, string titulo, string autor, string isbn, DateTime? fechaPublicacion, string rutaArchivo, int idCategoria)
        {
            libroDatos.ActualizarLibro(idLibro, titulo, autor, isbn, fechaPublicacion, rutaArchivo, idCategoria);
        }

       
        /// Elimina un libro de la base de datos según su Id.
       
        /// <param name="idLibro">Id del libro a eliminar.</param>
        public void EliminarLibro(int idLibro)
        {
            libroDatos.EliminarLibro(idLibro);
        }
    }
}