using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.Web
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public int IdRol { get; set; }
    }
}