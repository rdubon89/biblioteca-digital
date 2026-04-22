using System;
using System.Collections.Generic;

namespace BibliotecaDigital.Web
{
    
    /// Página principal del sistema Biblioteca Digital.
    /// Muestra el catálogo de libros, habilita accesos rápidos
    /// según el rol del usuario y permite cerrar la sesión.
    
    public partial class Home : System.Web.UI.Page
    {
        
        /// Evento de carga de la página.
        /// Verifica si el usuario tiene sesión activa y,
        /// en la primera carga, muestra los paneles de acceso
        /// y carga los libros del catálogo.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica que el usuario haya iniciado sesión
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Solo ejecutar la carga inicial una vez
            if (!IsPostBack)
            {
                MostrarPanelAccesoRapido();
                CargarLibros();
            }
        }

        
        /// Muestra los paneles de acceso rápido según el rol.
        /// - Administrador: libros, dashboard y usuarios
        /// - Bibliotecario: libros y dashboard
        /// - Ejecutivo: libros y dashboard
        /// - Usuario: sin paneles administrativos
        
        private void MostrarPanelAccesoRapido()
        {
            string rol = Session["Rol"] != null ? Session["Rol"].ToString() : string.Empty;

            // Panel general de libros/dashboard
            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Ejecutivo", StringComparison.OrdinalIgnoreCase))
            {
                pnlAdminLibros.Visible = true;
            }
            else
            {
                pnlAdminLibros.Visible = false;
            }

            // Panel exclusivo para administración de usuarios
            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                pnlAdminUsuarios.Visible = true;
            }
            else
            {
                pnlAdminUsuarios.Visible = false;
            }
        }


        /// Consulta todos los libros desde la base de datos
        /// y los muestra en el Repeater del catálogo.
        /// Si no existen libros, muestra un mensaje informativo.

        private void CargarLibros()
        {
            try
            {
                using (var client = ApiHelper.CrearCliente())
                {
                    // Llamar a la API
                    var response = client.GetAsync("api/libros").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblSinLibros.Text = "Error al obtener los libros desde el backend.";
                        return;
                    }

                    // Leer JSON
                    var json = response.Content.ReadAsStringAsync().Result;

                    // Deserializar
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var libros = serializer.Deserialize<List<Libro>>(json);

                    rptLibros.DataSource = libros;
                    rptLibros.DataBind();

                    if (libros == null || libros.Count == 0)
                    {
                        lblSinLibros.Text = "No hay libros disponibles.";
                    }
                    else
                    {
                        lblSinLibros.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSinLibros.Text = "Error al cargar libros: " + ex.Message;
            }
        }


        /// Cierra la sesión actual del usuario
        /// y lo redirige nuevamente al login.

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}