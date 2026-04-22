using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    /// Representa los indicadores principales del dashboard.
    public class DashboardResumen
    {
        public int TotalLibros { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalAccesos { get; set; }
    }
}