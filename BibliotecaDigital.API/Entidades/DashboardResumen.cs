namespace BibliotecaDigital.API.Entidades
{
   
    /// Entidad con el resumen general del dashboard.
    
    public class DashboardResumen
    {
        public int TotalLibros { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalAccesos { get; set; }
    }
}