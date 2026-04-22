using System.Collections.Generic;
using BibliotecaDigital.API.Datos;
using BibliotecaDigital.API.Entidades;

namespace BibliotecaDigital.API.Negocio
{
    
    /// Capa de lógica de negocio del dashboard.
    /// Se encarga de coordinar la información estadística
    /// que será mostrada en la interfaz.
    
    public class DashboardNegocio
    {
        // Instancia de acceso a datos del dashboard
        private DashboardDatos dashboardDatos = new DashboardDatos();

        
        /// Obtiene el resumen general del sistema:
        /// total de libros, usuarios, categorías y accesos.
       
        /// <returns>Objeto con los indicadores principales del dashboard.</returns>
        public DashboardResumen ObtenerResumenGeneral()
        {
            return dashboardDatos.ObtenerResumenGeneral();
        }

       
        /// Obtiene únicamente el total de libros registrados.
        
        /// <returns>Total de libros.</returns>
        public int ObtenerTotalLibros()
        {
            return dashboardDatos.ObtenerTotalLibros();
        }

        
        /// Obtiene la lista de los accesos más recientes al sistema.
        
        /// <returns>Lista de accesos recientes.</returns>
        public List<DashboardAccesoReciente> ObtenerUltimosAccesos()
        {
            return dashboardDatos.ObtenerUltimosAccesos();
        }

       
        /// Obtiene la lista de los libros registrados más recientemente.
        
        /// <returns>Lista de libros recientes.</returns>
        public List<DashboardLibroReciente> ObtenerUltimosLibros()
        {
            return dashboardDatos.ObtenerUltimosLibros();
        }

       
        /// Obtiene la cantidad de libros agrupados por categoría.
  
        /// <returns>Lista con categoría y total de libros.</returns>
        public List<DashboardLibroCategoria> ObtenerLibrosPorCategoria()
        {
            return dashboardDatos.ObtenerLibrosPorCategoria();
        }
    }
}