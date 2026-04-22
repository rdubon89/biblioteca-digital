using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BibliotecaDigital.API.Datos;
using BibliotecaDigital.API.Entidades;
using BibliotecaDigital.API.Helpers;


namespace BibliotecaDigital.API.Negocio
{
    
    /// Capa de lógica de negocio para usuarios.
   
    public class UsuarioNegocio
    {
        private UsuarioDatos usuarioDatos = new UsuarioDatos();

        
        /// Valida el login del usuario.
        
        public Usuario Login(string correo, string password)
        {
            string passwordHash = SeguridadHelper.ObtenerSHA256(password);
            return usuarioDatos.ValidarLogin(correo, passwordHash);
        }

        
        /// Registra la fecha del último acceso.
        
        public void RegistrarUltimoAcceso(int idUsuario)
        {
            usuarioDatos.ActualizarUltimoAcceso(idUsuario);
        }

        
        /// Registra el historial de acceso.
        
        public void RegistrarHistorial(int idUsuario, string direccionIP, bool exitoso)
        {
            usuarioDatos.RegistrarHistorialAcceso(idUsuario, direccionIP, exitoso);
        }

        
        /// Registra un usuario nuevo.
        
        public bool RegistrarUsuario(string nombre, string correo, string password)
        {
            if (usuarioDatos.ExisteCorreo(correo))
                return false;

            string passwordHash = SeguridadHelper.ObtenerSHA256(password);
            usuarioDatos.InsertarUsuario(nombre, correo, passwordHash);
            return true;
        }

        
        /// Obtiene todos los usuarios.
       
        public List<Usuario> ObtenerTodos()
        {
            return usuarioDatos.ObtenerTodos();
        }

        
        /// Obtiene un usuario por su Id.
        
        public Usuario ObtenerPorId(int idUsuario)
        {
            return usuarioDatos.ObtenerPorId(idUsuario);
        }

      
        /// Actualiza un usuario existente.
        
        public void ActualizarUsuario(int idUsuario, string nombre, string correo, int idRol)
        {
            usuarioDatos.ActualizarUsuario(idUsuario, nombre, correo, idRol);
        }

        
        /// Obtiene el catálogo de roles.
        
        public List<RolItem> ObtenerRoles()
        {
            return usuarioDatos.ObtenerRoles();
        }

        public bool InsertarUsuarioConRol(string nombre, string correo, string password, int idRol)
        {
            if (usuarioDatos.ExisteCorreo(correo))
                return false;

            string passwordHash = SeguridadHelper.ObtenerSHA256(password);
            usuarioDatos.InsertarUsuarioConRol(nombre, correo, passwordHash, idRol);
            return true;
        }

        public bool EliminarUsuario(int idUsuario)
        {
            return usuarioDatos.EliminarUsuario(idUsuario);
        }
    }
}