namespace BibliotecaDigital.API.Models
{
    
    /// Modelo para insertar un nuevo usuario desde la API.
    
    public class UsuarioInsertRequest
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }
    }
}