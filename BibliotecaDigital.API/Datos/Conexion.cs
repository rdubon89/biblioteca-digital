using System.Configuration;
using System.Data.SqlClient;

namespace BibliotecaDigital.API.Datos
{
    
    /// Clase encargada de obtener la conexión a la base de datos.
    
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            string cadena = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
            return new SqlConnection(cadena);
        }
    }
}