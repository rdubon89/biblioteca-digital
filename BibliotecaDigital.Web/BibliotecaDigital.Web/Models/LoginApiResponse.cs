namespace BibliotecaDigital.Web
{
    
    /// Modelo para recibir la respuesta del login desde la API.
    
    public class LoginApiResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public UsuarioApi usuario { get; set; }
    }

    public class UsuarioApi
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public int IdRol { get; set; }
    }
}