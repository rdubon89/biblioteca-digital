using System;
using System.Web.Http;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
   
    /// Controlador de dashboard.
    /// Expone los datos estadísticos del sistema para el frontend.
    
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        
        /// Obtiene el resumen general del sistema.
        
        [HttpGet]
        [Route("resumen")]
        public IHttpActionResult GetResumen()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();
                var resumen = negocio.ObtenerResumenGeneral();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene el total de libros.
        
        [HttpGet]
        [Route("total-libros")]
        public IHttpActionResult GetTotalLibros()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();
                int total = negocio.ObtenerTotalLibros();
                return Ok(new { TotalLibros = total });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene la lista de accesos recientes.
        
        [HttpGet]
        [Route("ultimos-accesos")]
        public IHttpActionResult GetUltimosAccesos()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();
                var accesos = negocio.ObtenerUltimosAccesos();
                return Ok(accesos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene la lista de libros recientes.
        
        [HttpGet]
        [Route("ultimos-libros")]
        public IHttpActionResult GetUltimosLibros()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();
                var libros = negocio.ObtenerUltimosLibros();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene la cantidad de libros por categoría.
        
        [HttpGet]
        [Route("libros-por-categoria")]
        public IHttpActionResult GetLibrosPorCategoria()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();
                var categorias = negocio.ObtenerLibrosPorCategoria();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}