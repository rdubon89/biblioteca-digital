using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibliotecaDigital.API.Models
{
    
    /// Modelo para recibir las credenciales del login.
    
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}