using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;

namespace BibliotecaDigital.Web
{
    
    /// Página principal del sistema Biblioteca Digital.
    /// Muestra el catálogo de libros y accesos rápidos según el rol del usuario.
    
    public partial class Home : System.Web.UI.Page
    {
        
        /// URL base de la API.
        /// Se utiliza desde el archivo ASPX para generar enlaces de abrir y descargar libros.
        
        public string ApiBaseUrl { get; private set; }

        
        /// Evento de carga de la página.
        /// Valida sesión activa, configura accesos rápidos y carga el catálogo.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ApiBaseUrl = ObtenerApiBaseUrl();

            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                MostrarPanelAccesoRapido();
                CargarLibros();
            }
        }

        
        /// Obtiene y normaliza la URL base de la API desde Web.config.
        
        private string ObtenerApiBaseUrl()
        {
            string url = ConfigurationManager.AppSettings["ApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                return "https://localhost:44341/";
            }

            return url.EndsWith("/") ? url : url + "/";
        }

        
        /// Muestra los accesos rápidos según el rol del usuario.
        /// Administrador:
        /// - Dashboard
        /// - Administración de libros
        /// - Administración de usuarios
        /// 
        /// Bibliotecario:
        /// - Dashboard
        /// - Administración de libros
        /// 
        /// Ejecutivo:
        /// - Dashboard
        /// 
        /// User:
        /// - Solo catálogo
       
        private void MostrarPanelAccesoRapido()
        {
            string rol = Session["Rol"] != null
                ? Session["Rol"].ToString()
                : string.Empty;

            phDashboard.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Ejecutivo", StringComparison.OrdinalIgnoreCase);

            phAdminLibros.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase);

            phAdminUsuarios.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }

       
        /// Consulta todos los libros desde la API y los muestra en el Repeater.
        
        private void CargarLibros()
        {
            try
            {
                using (var client = ApiHelper.CrearCliente())
                {
                    var response = client.GetAsync("api/libros").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblSinLibros.Text = "Error al obtener los libros desde el backend.";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Libro> libros = serializer.Deserialize<List<Libro>>(json);

                    rptLibros.DataSource = libros;
                    rptLibros.DataBind();

                    lblSinLibros.Text = libros == null || libros.Count == 0
                        ? "No hay libros disponibles."
                        : string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblSinLibros.Text = "Error al cargar libros: " + ex.Message;
            }
        }
    }
}