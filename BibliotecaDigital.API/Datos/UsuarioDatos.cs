using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Datos
{
    
    /// Capa de acceso a datos para usuarios.
    /// Maneja login, registro, consulta y actualización de usuarios.
    
    public class UsuarioDatos
    {
        
        /// Valida el login de un usuario por correo y hash de contraseña.
        
        public Usuario ValidarLogin(string correo, string passwordHash)
        {
            Usuario usuario = null;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_ValidarLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        PasswordHash = dr["PasswordHash"].ToString(),
                        Rol = dr["Rol"].ToString(),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        UltimoAcceso = dr["UltimoAcceso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UltimoAcceso"])
                    };
                }
            }

            return usuario;
        }

       
        /// Actualiza la fecha del último acceso del usuario.
        
        public void ActualizarUltimoAcceso(int idUsuario)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_ActualizarUltimoAcceso", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        /// Inserta un registro en el historial de acceso.
      
        public void RegistrarHistorialAcceso(int idUsuario, string direccionIP, bool exitoso)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_HistorialAcceso_Insertar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@DireccionIP", (object)direccionIP ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Exitoso", exitoso);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        /// Verifica si un correo ya existe.
        
        public bool ExisteCorreo(string correo)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_ExisteCorreo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);
                con.Open();
                int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                return cantidad > 0;
            }
        }

        
        /// Inserta un nuevo usuario.
        
        public void InsertarUsuario(string nombre, string correo, string passwordHash)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_Insertar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@IdRol", 4);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        /// Obtiene todos los usuarios registrados.
       
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_ObtenerTodos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Rol = dr["Rol"].ToString(),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        UltimoAcceso = dr["UltimoAcceso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UltimoAcceso"])
                    };

                    lista.Add(usuario);
                }
            }

            return lista;
        }

        
        /// Obtiene un usuario por su Id.
       
        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_ObtenerPorId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Rol = dr["Rol"].ToString(),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        UltimoAcceso = dr["UltimoAcceso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["UltimoAcceso"])
                    };
                }
            }

            return usuario;
        }

        
        /// Actualiza los datos básicos de un usuario.
       
        public void ActualizarUsuario(int idUsuario, string nombre, string correo, int idRol)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_Actualizar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        /// Obtiene el catálogo de roles.
        
        public List<RolItem> ObtenerRoles()
        {
            List<RolItem> lista = new List<RolItem>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Rol_ObtenerTodos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RolItem item = new RolItem
                    {
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        Nombre = dr["Nombre"].ToString()
                    };

                    lista.Add(item);
                }
            }

            return lista;
        }
        public void InsertarUsuarioConRol(string nombre, string correo, string passwordHash, int idRol)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_Insertar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool EliminarUsuario(int idUsuario)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Usuario_Eliminar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}