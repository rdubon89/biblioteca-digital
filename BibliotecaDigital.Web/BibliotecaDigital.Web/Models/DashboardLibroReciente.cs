using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    
    /// Representa un libro reciente para mostrar en el dashboard.
    
    public class DashboardLibroReciente
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string Categoria { get; set; }
    }
}