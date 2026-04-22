using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    public class HistorialAcceso
    {
        /// Representa un registro de acceso al sistema.
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime FechaAcceso { get; set; }
        public string DireccionIP { get; set; }
    }
}