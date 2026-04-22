namespace BibliotecaDigital.API.Entidades
{
    
    /// Entidad para mostrar el total de libros agrupados por categoría.
   
    public class DashboardLibroCategoria
    {
        public string Categoria { get; set; }
        public int TotalLibros { get; set; }
    }
}