using System;
using System.Web.Http;
using BibliotecaDigital.API.Models;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    
    /// Controlador para exponer categorías del sistema.
    
    [RoutePrefix("api/categorias")]
    public class CategoriasController : ApiController
    {
        
        /// Obtiene todas las categorías.
        
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCategorias()
        {
            try
            {
                CategoriaNegocio negocio = new CategoriaNegocio();
                var categorias = negocio.ObtenerTodas();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Inserta una nueva categoría.
        
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertarCategoria(CategoriaInsertRequest request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.RutaFisica))
                {
                    return BadRequest("Debe enviar nombre y ruta física.");
                }

                CategoriaNegocio negocio = new CategoriaNegocio();
                int idCategoria = negocio.InsertarCategoria(
                    request.Nombre.Trim(),
                    request.RutaFisica.Trim()
                );

                return Ok(new
                {
                    success = true,
                    message = "Categoría creada correctamente.",
                    idCategoria = idCategoria
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene una categoría por nombre.
        
        [HttpGet]
        [Route("por-nombre")]
        public IHttpActionResult GetCategoriaPorNombre(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return BadRequest("Debe enviar el nombre.");

                CategoriaNegocio negocio = new CategoriaNegocio();
                var categoria = negocio.ObtenerPorNombre(nombre.Trim());

                if (categoria == null)
                    return NotFound();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}