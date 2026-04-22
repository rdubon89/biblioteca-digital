using System;

namespace BibliotecaDigital.API.Entidades
{
    
    /// Entidad para mostrar libros recientes en el dashboard.
    
    public class DashboardLibroReciente
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string Categoria { get; set; }
    }
}