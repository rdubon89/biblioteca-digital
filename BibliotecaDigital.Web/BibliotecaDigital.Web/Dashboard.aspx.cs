using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace BibliotecaDigital.Web
{
    
    /// Página de dashboard principal del sistema.
    /// Consume la API del backend para mostrar indicadores y listados.
   
    public partial class Dashboard : System.Web.UI.Page
    {
        
        /// Evento de carga de la página.
        /// Valida sesión activa y carga el dashboard según el rol.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar sesión activa
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Solo cargar una vez
            if (!IsPostBack)
            {
                ConfigurarDashboardPorRol();
            }
        }

        
        /// Define qué panel mostrar según el rol del usuario.
        
        private void ConfigurarDashboardPorRol()
        {
            string rol = Session["Rol"] != null ? Session["Rol"].ToString() : string.Empty;

            // Ocultar todos los paneles antes de decidir
            pnlDashboardCompleto.Visible = false;
            pnlDashboardLibros.Visible = false;
            pnlSinPermiso.Visible = false;

            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase))
            {
                pnlDashboardCompleto.Visible = true;
                CargarDashboardCompleto();
            }
            else if (rol.Equals("Ejecutivo", StringComparison.OrdinalIgnoreCase))
            {
                pnlDashboardLibros.Visible = true;
                CargarDashboardLibros();
            }
            else
            {
                pnlSinPermiso.Visible = true;
            }
        }

        
        /// Carga el dashboard completo para Administrador y Bibliotecario.
       
        private void CargarDashboardCompleto()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    // Resumen general
                    HttpResponseMessage responseResumen = client.GetAsync("api/dashboard/resumen").Result;
                    if (responseResumen.IsSuccessStatusCode)
                    {
                        string jsonResumen = responseResumen.Content.ReadAsStringAsync().Result;
                        DashboardResumen resumen = serializer.Deserialize<DashboardResumen>(jsonResumen);

                        lblTotalLibros.Text = resumen.TotalLibros.ToString();
                        lblTotalUsuarios.Text = resumen.TotalUsuarios.ToString();
                        lblTotalCategorias.Text = resumen.TotalCategorias.ToString();
                        lblTotalAccesos.Text = resumen.TotalAccesos.ToString();
                    }

                    // Últimos accesos
                    HttpResponseMessage responseAccesos = client.GetAsync("api/dashboard/ultimos-accesos").Result;
                    if (responseAccesos.IsSuccessStatusCode)
                    {
                        string jsonAccesos = responseAccesos.Content.ReadAsStringAsync().Result;
                        List<DashboardAccesoReciente> accesos = serializer.Deserialize<List<DashboardAccesoReciente>>(jsonAccesos);

                        gvAccesos.DataSource = accesos;
                        gvAccesos.DataBind();
                    }

                    // Últimos libros
                    HttpResponseMessage responseLibros = client.GetAsync("api/dashboard/ultimos-libros").Result;
                    if (responseLibros.IsSuccessStatusCode)
                    {
                        string jsonLibros = responseLibros.Content.ReadAsStringAsync().Result;
                        List<DashboardLibroReciente> libros = serializer.Deserialize<List<DashboardLibroReciente>>(jsonLibros);

                        gvUltimosLibrosCompleto.DataSource = libros;
                        gvUltimosLibrosCompleto.DataBind();
                    }

                    // Libros por categoría
                    HttpResponseMessage responseCategorias = client.GetAsync("api/dashboard/libros-por-categoria").Result;
                    if (responseCategorias.IsSuccessStatusCode)
                    {
                        string jsonCategorias = responseCategorias.Content.ReadAsStringAsync().Result;
                        List<DashboardLibroCategoria> categorias = serializer.Deserialize<List<DashboardLibroCategoria>>(jsonCategorias);

                        rptLibrosCategoriaCompleto.DataSource = categorias;
                        rptLibrosCategoriaCompleto.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                pnlDashboardCompleto.Visible = false;
                pnlSinPermiso.Visible = true;
                lblErrorDashboard.Text = "Error al cargar el dashboard completo: " + ex.Message;
            }
        }

       
        /// Carga el dashboard reducido para el rol Ejecutivo.
       
        private void CargarDashboardLibros()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    // Total de libros
                    HttpResponseMessage responseTotal = client.GetAsync("api/dashboard/total-libros").Result;
                    if (responseTotal.IsSuccessStatusCode)
                    {
                        string jsonTotal = responseTotal.Content.ReadAsStringAsync().Result;
                        var totalObj = serializer.Deserialize<Dictionary<string, object>>(jsonTotal);

                        if (totalObj.ContainsKey("TotalLibros"))
                        {
                            lblTotalLibrosRol3.Text = totalObj["TotalLibros"].ToString();
                        }
                    }

                    // Últimos libros
                    HttpResponseMessage responseLibros = client.GetAsync("api/dashboard/ultimos-libros").Result;
                    if (responseLibros.IsSuccessStatusCode)
                    {
                        string jsonLibros = responseLibros.Content.ReadAsStringAsync().Result;
                        List<DashboardLibroReciente> libros = serializer.Deserialize<List<DashboardLibroReciente>>(jsonLibros);

                        gvUltimosLibrosRol3.DataSource = libros;
                        gvUltimosLibrosRol3.DataBind();
                    }

                    // Libros por categoría
                    HttpResponseMessage responseCategorias = client.GetAsync("api/dashboard/libros-por-categoria").Result;
                    if (responseCategorias.IsSuccessStatusCode)
                    {
                        string jsonCategorias = responseCategorias.Content.ReadAsStringAsync().Result;
                        List<DashboardLibroCategoria> categorias = serializer.Deserialize<List<DashboardLibroCategoria>>(jsonCategorias);

                        rptLibrosCategoriaRol3.DataSource = categorias;
                        rptLibrosCategoriaRol3.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                pnlDashboardLibros.Visible = false;
                pnlSinPermiso.Visible = true;
                lblErrorDashboard.Text = "Error al cargar el dashboard de libros: " + ex.Message;
            }
        }
    }
}