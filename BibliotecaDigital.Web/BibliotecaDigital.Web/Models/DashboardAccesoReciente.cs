using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    
    /// Representa un acceso reciente al sistema.
    
    public class DashboardAccesoReciente
    {
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public DateTime FechaAcceso { get; set; }
        public string DireccionIP { get; set; }
        public bool Exitoso { get; set; }
    }
}