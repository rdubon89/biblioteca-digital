using System;
using System.Net;
using System.Web.Http;
using BibliotecaDigital.API.Entidades;
using BibliotecaDigital.API.Models;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    
    /// Controlador encargado de la administración de usuarios.
    
    /// Este controlador está protegido con JWT y solo permite acceso
    /// a usuarios con rol Administrador.
    
    /// Rol 1 = Administrador
    
    [Authorize(Roles = "1")]
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        
        /// Obtiene todos los usuarios del sistema.
        
        /// Seguridad:
        /// Solo usuarios administradores pueden consultar esta información.
        /// Esto se controla mediante [Authorize(Roles = "1")] a nivel de controlador.
        
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsuarios()
        {
            try
            {
                // Se crea una instancia de la capa de negocio.
                // Esta capa centraliza la lógica relacionada con usuarios.
                UsuarioNegocio negocio = new UsuarioNegocio();

                // Se obtiene la lista completa de usuarios desde la capa de negocio.
                // Internamente, la capa de negocio consulta la capa de datos.
                var usuarios = negocio.ObtenerTodos();

                // Se devuelve la lista en formato JSON con código HTTP 200 OK.
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Si ocurre un error inesperado, se devuelve HTTP 500.
                return InternalServerError(ex);
            }
        }

        
        /// Obtiene un usuario específico por su Id.
       
        /// Seguridad:
        /// Solo administradores pueden consultar usuarios por Id.
        
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetUsuarioPorId(int id)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                // Se busca el usuario en base al Id recibido desde la URL.
                Usuario usuario = negocio.ObtenerPorId(id);

                // Si no existe el usuario, se devuelve HTTP 404 Not Found.
                if (usuario == null)
                    return NotFound();

                // Si existe, se devuelve el usuario encontrado.
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Actualiza la información de un usuario existente.
       
        /// Seguridad:
        /// Solo administradores pueden actualizar usuarios.
        
        /// Nota:
        /// Aquí se actualizan datos como nombre, correo y rol.
       
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ActualizarUsuario(int id, UsuarioUpdateRequest request)
        {
            try
            {
                // Validación del request.
                // Esto evita procesar datos vacíos, nulos o inválidos.
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    request.IdRol <= 0)
                {
                    return BadRequest("Debe enviar nombre, correo e IdRol válidos.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();

                // Se valida que el usuario que se quiere actualizar exista.
                Usuario usuario = negocio.ObtenerPorId(id);

                if (usuario == null)
                    return NotFound();

                // Se llama a la capa de negocio para actualizar el usuario.
                negocio.ActualizarUsuario(
                    id,
                    request.Nombre.Trim(),
                    request.Correo.Trim(),
                    request.IdRol
                );

                return Ok(new
                {
                    success = true,
                    message = "Usuario actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Inserta un nuevo usuario con un rol específico.
        
        /// Seguridad:
        /// Solo administradores pueden crear usuarios desde este endpoint.
        
        /// Diferencia con register:
        /// - Register crea usuarios con rol por defecto.
        /// - Este endpoint permite al administrador asignar un rol.
       
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertarUsuario(UsuarioInsertRequest request)
        {
            try
            {
                // Validación de datos mínimos requeridos.
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    request.IdRol <= 0)
                {
                    return BadRequest("Debe enviar nombre, correo, contraseña e IdRol válidos.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();

                // Se crea el usuario con el rol indicado por el administrador.
                bool creado = negocio.InsertarUsuarioConRol(
                    request.Nombre.Trim(),
                    request.Correo.Trim(),
                    request.Password.Trim(),
                    request.IdRol
                );

                // Si no se pudo crear, normalmente es porque el correo ya existe.
                if (!creado)
                {
                    return BadRequest("No se pudo crear el usuario. El correo ya existe.");
                }

                return Ok(new
                {
                    success = true,
                    message = "Usuario creado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Elimina un usuario del sistema.
       
        /// Regla de negocio:
        /// - Un administrador normal puede eliminar bibliotecarios, ejecutivos y usuarios generales.
        /// - Un administrador normal NO puede eliminar otro administrador.
        /// - Solo el usuario con correo superadmin@bibliomail.com puede eliminar administradores.
       
        /// Esto evita que un administrador elimine accidentalmente o maliciosamente
        /// a otros usuarios con el mismo nivel de privilegio.
        
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult EliminarUsuario(int id)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                // Primero se obtiene el usuario objetivo.
                // Este es el usuario que se intenta eliminar.
                Usuario usuarioAEliminar = negocio.ObtenerPorId(id);

                // Si el usuario no existe, se devuelve HTTP 404.
                if (usuarioAEliminar == null)
                    return NotFound();

                // User.Identity.Name viene del JWT.
                // En JwtHelper configuramos ClaimTypes.Name con el correo del usuario.
                // Por eso aquí podemos saber qué usuario está haciendo la acción.
                string correoUsuarioActual = User.Identity.Name;

                // Validamos si el usuario que se quiere eliminar es administrador.
                bool usuarioAEliminarEsAdministrador = usuarioAEliminar.IdRol == 1;

                // Validamos si quien está haciendo la acción es el superadministrador.
                bool usuarioActualEsSuperAdmin = correoUsuarioActual.Equals(
                    "superadmin@bibliomail.com",
                    StringComparison.OrdinalIgnoreCase
                );

                // Regla crítica:
                // Si el usuario objetivo es administrador y quien ejecuta la acción
                // NO es el superadmin, se bloquea la eliminación.
                if (usuarioAEliminarEsAdministrador && !usuarioActualEsSuperAdmin)
                {
                    return Content(HttpStatusCode.Forbidden, new
                    {
                        success = false,
                        message = "Solo el superadministrador puede eliminar usuarios administradores."
                    });
                }

                // Si pasa las validaciones, se elimina el usuario.
                bool eliminado = negocio.EliminarUsuario(id);

                if (!eliminado)
                    return BadRequest("No se pudo eliminar el usuario.");

                return Ok(new
                {
                    success = true,
                    message = "Usuario eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}