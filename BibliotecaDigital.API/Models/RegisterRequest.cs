namespace BibliotecaDigital.API.Models
{
    
    /// Modelo para registrar un nuevo usuario desde la API.
   
    public class RegisterRequest
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}