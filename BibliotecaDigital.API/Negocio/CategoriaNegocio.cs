using System.Collections.Generic;
using BibliotecaDigital.API.Datos;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Negocio
{
    
    /// Lógica de negocio para categorías.
   
    public class CategoriaNegocio
    {
        private CategoriaDatos categoriaDatos = new CategoriaDatos();

        public List<Categoria> ObtenerTodas()
        {
            return categoriaDatos.ObtenerTodas();
        }

        public Categoria ObtenerPorId(int idCategoria)
        {
            return categoriaDatos.ObtenerPorId(idCategoria);
        }

        
        /// Inserta una nueva categoría y devuelve su Id.
        
        public int InsertarCategoria(string nombre, string rutaFisica)
        {
            return categoriaDatos.InsertarCategoria(nombre, rutaFisica);
        }

        /// Obtiene una categoría por nombre exacto.
        public Categoria ObtenerPorNombre(string nombre)
        {
            return categoriaDatos.ObtenerPorNombre(nombre);
        }
    }
}