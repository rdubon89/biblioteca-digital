namespace BibliotecaDigital.API.Models
{
    
    /// Modelo para actualizar los datos básicos de un usuario.
   
    public class UsuarioUpdateRequest
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public int IdRol { get; set; }
    }
}