using System;
using System.Web.Http;
using BibliotecaDigital.API.Entidades;
using BibliotecaDigital.API.Models;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsuarios()
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                var usuarios = negocio.ObtenerTodos();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetUsuarioPorId(int id)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = negocio.ObtenerPorId(id);

                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ActualizarUsuario(int id, UsuarioUpdateRequest request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    request.IdRol <= 0)
                {
                    return BadRequest("Debe enviar nombre, correo e IdRol válidos.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = negocio.ObtenerPorId(id);

                if (usuario == null)
                    return NotFound();

                negocio.ActualizarUsuario(id, request.Nombre.Trim(), request.Correo.Trim(), request.IdRol);

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

        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertarUsuario(UsuarioInsertRequest request)
        {
            try
            {
                if (request == null ||
                    string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    request.IdRol <= 0)
                {
                    return BadRequest("Debe enviar nombre, correo, contraseña e IdRol válidos.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                bool creado = negocio.InsertarUsuarioConRol(
                    request.Nombre.Trim(),
                    request.Correo.Trim(),
                    request.Password.Trim(),
                    request.IdRol
                );

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

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult EliminarUsuario(int id)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
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