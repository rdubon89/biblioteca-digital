using System;
using System.Web.Http;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    
    /// Controlador para exponer el catálogo de roles.
    
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
    {
        
        /// Obtiene todos los roles registrados.
        
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetRoles()
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                var roles = negocio.ObtenerRoles();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}