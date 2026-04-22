using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    
    /// Entidad que representa un libro dentro del sistema.
   
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string RutaArchivo { get; set; }
        public string Categoria { get; set; }
        public int IdCategoria { get; set; }
    }
}