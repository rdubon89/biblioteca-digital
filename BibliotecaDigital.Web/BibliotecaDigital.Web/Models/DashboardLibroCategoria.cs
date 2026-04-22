using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    
    /// Representa la cantidad de libros agrupados por categoría.
    
    public class DashboardLibroCategoria
    {
        public string Categoria { get; set; }
        public int TotalLibros { get; set; }
    }
}