namespace BibliotecaDigital.API.Models
{
    
    /// Modelo para insertar una nueva categoría desde la API.
    
    public class CategoriaInsertRequest
    {
        public string Nombre { get; set; }
        public string RutaFisica { get; set; }
    }
}