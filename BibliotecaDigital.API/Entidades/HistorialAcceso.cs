using System;

namespace BibliotecaDigital.API.Entidades
{
    
    /// Entidad que representa un registro de acceso al sistema.
    
    public class HistorialAcceso
    {
        public int IdHistorial { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaAcceso { get; set; }
        public string DireccionIP { get; set; }
        public bool Exitoso { get; set; }
    }
}