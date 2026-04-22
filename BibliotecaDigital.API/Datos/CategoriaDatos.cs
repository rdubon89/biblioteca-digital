using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Datos
{
    public class CategoriaDatos
    {
        public List<Categoria> ObtenerTodas()
        {
            List<Categoria> lista = new List<Categoria>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Categoria_ObtenerTodos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Categoria categoria = new Categoria
                    {
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                        Nombre = dr["Nombre"].ToString(),
                        RutaFisica = dr["RutaFisica"].ToString()
                    };

                    lista.Add(categoria);
                }
            }

            return lista;
        }

        public Categoria ObtenerPorId(int idCategoria)
        {
            Categoria categoria = null;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Categoria_ObtenerPorId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    categoria = new Categoria
                    {
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                        Nombre = dr["Nombre"].ToString(),
                        RutaFisica = dr["RutaFisica"].ToString()
                    };
                }
            }

            return categoria;
        }

        
        /// Inserta una nueva categoría y devuelve su Id.
        
        public int InsertarCategoria(string nombre, string rutaFisica)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Categoria_Insertar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@RutaFisica", rutaFisica);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        
        /// Obtiene una categoría por su nombre exacto.
        
        public Categoria ObtenerPorNombre(string nombre)
        {
            Categoria categoria = null;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Categoria_ObtenerPorNombre", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    categoria = new Categoria
                    {
                        IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                        Nombre = dr["Nombre"].ToString(),
                        RutaFisica = dr["RutaFisica"].ToString()
                    };
                }
            }

            return categoria;
        }
    }
}