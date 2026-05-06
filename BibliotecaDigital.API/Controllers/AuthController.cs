using System;
using System.Web;
using System.Web.Http;
using BibliotecaDigital.API.Entidades;
using BibliotecaDigital.API.Helpers;
using BibliotecaDigital.API.Models;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    /// Controlador encargado de la autenticación de usuarios.
    /// Valida credenciales y registra el acceso exitoso.

    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        /// Valida las credenciales de un usuario y devuelve su información básica.
        /// Además, registra el último acceso y el historial de acceso exitoso.

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginRequest request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest("Debe enviar correo y contraseña.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = negocio.Login(request.Correo.Trim(), request.Password.Trim());

                if (usuario == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Credenciales incorrectas"
                    });
                }

                string direccionIP = HttpContext.Current != null &&
                                     HttpContext.Current.Request != null
                    ? HttpContext.Current.Request.UserHostAddress
                    : null;

                negocio.RegistrarUltimoAcceso(usuario.IdUsuario);
                negocio.RegistrarHistorial(usuario.IdUsuario, direccionIP, true);

                string token = JwtHelper.GenerarToken(usuario);

                return Ok(new
                {
                    success = true,
                    message = "Login exitoso",
                    token = token,
                    usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol,
                        usuario.IdRol
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// Registra un nuevo usuario en el sistema.
        /// El usuario se registra con el rol por defecto definido en la capa de datos.

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(RegisterRequest request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest("Debe enviar nombre, correo y contraseña.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                bool registrado = negocio.RegistrarUsuario(
                    request.Nombre.Trim(),
                    request.Correo.Trim(),
                    request.Password.Trim()
                );

                if (!registrado)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No se pudo registrar. El correo ya existe."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Usuario registrado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}