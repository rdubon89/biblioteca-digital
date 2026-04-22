using System;

namespace BibliotecaDigital.API.Entidades
{
    
    /// Entidad para mostrar accesos recientes en el dashboard.
    
    public class DashboardAccesoReciente
    {
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public DateTime FechaAcceso { get; set; }
        public string DireccionIP { get; set; }
        public bool Exitoso { get; set; }
    }
}