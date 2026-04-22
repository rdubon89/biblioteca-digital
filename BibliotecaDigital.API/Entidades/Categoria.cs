namespace BibliotecaDigital.API.Entidades
{
    
    /// Entidad que representa una categoría de libros.
    
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string RutaFisica { get; set; }
    }
}