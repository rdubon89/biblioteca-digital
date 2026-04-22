using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Datos
{
   
    /// Capa de acceso a datos del dashboard.
    /// Ejecuta procedimientos almacenados para obtener
    /// información estadística del sistema.
    
    public class DashboardDatos
    {
        
        /// Obtiene el resumen general del sistema.
        
        /// <returns>Objeto con totales generales del dashboard.</returns>
        public DashboardResumen ObtenerResumenGeneral()
        {
            DashboardResumen resumen = new DashboardResumen();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Dashboard_ResumenGeneral", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    resumen.TotalLibros = Convert.ToInt32(dr["TotalLibros"]);
                    resumen.TotalUsuarios = Convert.ToInt32(dr["TotalUsuarios"]);
                    resumen.TotalCategorias = Convert.ToInt32(dr["TotalCategorias"]);
                    resumen.TotalAccesos = Convert.ToInt32(dr["TotalAccesos"]);
                }
            }

            return resumen;
        }

      
        /// Obtiene el total de libros registrados.
        
        /// <returns>Total de libros.</returns>
        public int ObtenerTotalLibros()
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Dashboard_TotalLibros", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        
        /// Obtiene los últimos accesos registrados en el sistema.
        
        /// <returns>Lista de accesos recientes.</returns>
        public List<DashboardAccesoReciente> ObtenerUltimosAccesos()
        {
            List<DashboardAccesoReciente> lista = new List<DashboardAccesoReciente>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Dashboard_UltimosAccesos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DashboardAccesoReciente acceso = new DashboardAccesoReciente
                    {
                        Usuario = dr["Usuario"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        FechaAcceso = Convert.ToDateTime(dr["FechaAcceso"]),
                        DireccionIP = dr["DireccionIP"] == DBNull.Value ? "" : dr["DireccionIP"].ToString(),
                        Exitoso = Convert.ToBoolean(dr["Exitoso"])
                    };

                    lista.Add(acceso);
                }
            }

            return lista;
        }

       
        /// Obtiene los últimos libros registrados en el sistema.
        
        /// <returns>Lista de libros recientes.</returns>
        public List<DashboardLibroReciente> ObtenerUltimosLibros()
        {
            List<DashboardLibroReciente> lista = new List<DashboardLibroReciente>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Dashboard_UltimosLibros", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DashboardLibroReciente libro = new DashboardLibroReciente
                    {
                        IdLibro = Convert.ToInt32(dr["IdLibro"]),
                        Titulo = dr["Titulo"].ToString(),
                        Autor = dr["Autor"].ToString(),
                        FechaPublicacion = dr["FechaPublicacion"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(dr["FechaPublicacion"]),
                        Categoria = dr["Categoria"].ToString()
                    };

                    lista.Add(libro);
                }
            }

            return lista;
        }

       
        /// Obtiene la cantidad de libros agrupados por categoría.
       
        /// <returns>Lista de categorías con su total de libros.</returns>
        public List<DashboardLibroCategoria> ObtenerLibrosPorCategoria()
        {
            List<DashboardLibroCategoria> lista = new List<DashboardLibroCategoria>();

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Biblioteca.sp_Dashboard_LibrosPorCategoria", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DashboardLibroCategoria item = new DashboardLibroCategoria
                    {
                        Categoria = dr["Categoria"].ToString(),
                        TotalLibros = Convert.ToInt32(dr["TotalLibros"])
                    };

                    lista.Add(item);
                }
            }

            return lista;
        }
    }
}