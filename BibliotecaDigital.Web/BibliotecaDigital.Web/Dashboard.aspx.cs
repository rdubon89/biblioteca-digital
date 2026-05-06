using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace BibliotecaDigital.Web
{
    
    /// Página principal del dashboard administrativo.
    /// Consume endpoints de la API para mostrar métricas, últimos accesos,
    /// últimos libros y distribución de libros por categoría.
 
    /// Permisos:
    /// - Administrador: dashboard completo.
    /// - Bibliotecario: dashboard completo.
    /// - Ejecutivo: dashboard reducido de libros.
    /// - User: sin acceso al dashboard administrativo.
    
    public partial class Dashboard : System.Web.UI.Page
    {
        
        /// Valida la sesión activa y carga el dashboard según el rol del usuario.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ConfigurarDashboardPorRol();
            }
        }

        
        /// Determina qué versión del dashboard debe visualizar el usuario autenticado.
        
        private void ConfigurarDashboardPorRol()
        {
            string rol = ObtenerRolActual();

            OcultarPaneles();

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
                MostrarPanelSinPermiso("No tiene permisos para visualizar el dashboard administrativo.");
            }
        }

        
        /// Obtiene el rol almacenado en sesión.
        
        private string ObtenerRolActual()
        {
            return Session["Rol"] != null ? Session["Rol"].ToString() : string.Empty;
        }

        
        /// Oculta todos los paneles del dashboard antes de mostrar el correspondiente.
        
        private void OcultarPaneles()
        {
            pnlDashboardCompleto.Visible = false;
            pnlDashboardLibros.Visible = false;
            pnlSinPermiso.Visible = false;
            lblErrorDashboard.Text = string.Empty;
        }

        
        /// Muestra el panel de error o sin permiso.
        
        private void MostrarPanelSinPermiso(string mensaje)
        {
            pnlDashboardCompleto.Visible = false;
            pnlDashboardLibros.Visible = false;
            pnlSinPermiso.Visible = true;
            lblErrorDashboard.Text = mensaje;
        }

      
        /// Carga el dashboard completo para Administrador y Bibliotecario.
        /// Incluye resumen general, últimos accesos, últimos libros y libros por categoría.
        
        private void CargarDashboardCompleto()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    CargarResumenGeneral(client, serializer);
                    CargarUltimosAccesos(client, serializer);
                    CargarUltimosLibrosCompleto(client, serializer);
                    CargarLibrosPorCategoriaCompleto(client, serializer);
                }
            }
            catch (Exception ex)
            {
                MostrarPanelSinPermiso("Error al cargar el dashboard completo: " + ex.Message);
            }
        }

        
        /// Carga el dashboard reducido para Ejecutivo.
        /// Incluye total de libros, últimos libros y libros por categoría.
        
        private void CargarDashboardLibros()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    CargarTotalLibrosEjecutivo(client, serializer);
                    CargarUltimosLibrosEjecutivo(client, serializer);
                    CargarLibrosPorCategoriaEjecutivo(client, serializer);
                }
            }
            catch (Exception ex)
            {
                MostrarPanelSinPermiso("Error al cargar el dashboard de libros: " + ex.Message);
            }
        }

        
        /// Consume api/dashboard/resumen y llena los indicadores principales.
        
        private void CargarResumenGeneral(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/resumen").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo cargar el resumen general.");

            string json = response.Content.ReadAsStringAsync().Result;
            DashboardResumen resumen = serializer.Deserialize<DashboardResumen>(json);

            if (resumen == null)
                throw new Exception("La API no devolvió datos de resumen.");

            lblTotalLibros.Text = resumen.TotalLibros.ToString();
            lblTotalUsuarios.Text = resumen.TotalUsuarios.ToString();
            lblTotalCategorias.Text = resumen.TotalCategorias.ToString();
            lblTotalAccesos.Text = resumen.TotalAccesos.ToString();
        }

        
        /// Consume api/dashboard/ultimos-accesos y muestra la auditoría reciente.
        
        private void CargarUltimosAccesos(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/ultimos-accesos").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudieron cargar los últimos accesos.");

            string json = response.Content.ReadAsStringAsync().Result;
            List<DashboardAccesoReciente> accesos =
                serializer.Deserialize<List<DashboardAccesoReciente>>(json);

            gvAccesos.DataSource = accesos;
            gvAccesos.DataBind();
        }

        
        /// Consume api/dashboard/ultimos-libros y llena el GridView del dashboard completo.
        
        private void CargarUltimosLibrosCompleto(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/ultimos-libros").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudieron cargar los últimos libros.");

            string json = response.Content.ReadAsStringAsync().Result;
            List<DashboardLibroReciente> libros =
                serializer.Deserialize<List<DashboardLibroReciente>>(json);

            gvUltimosLibrosCompleto.DataSource = libros;
            gvUltimosLibrosCompleto.DataBind();
        }

        
        /// Consume api/dashboard/libros-por-categoria y llena el repetidor del dashboard completo.
        
        private void CargarLibrosPorCategoriaCompleto(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/libros-por-categoria").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudieron cargar los libros por categoría.");

            string json = response.Content.ReadAsStringAsync().Result;
            List<DashboardLibroCategoria> categorias =
                serializer.Deserialize<List<DashboardLibroCategoria>>(json);

            rptLibrosCategoriaCompleto.DataSource = categorias;
            rptLibrosCategoriaCompleto.DataBind();
        }

        
        /// Consume api/dashboard/total-libros y muestra el total para Ejecutivo.
        
        private void CargarTotalLibrosEjecutivo(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/total-libros").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo cargar el total de libros.");

            string json = response.Content.ReadAsStringAsync().Result;
            Dictionary<string, object> totalObj =
                serializer.Deserialize<Dictionary<string, object>>(json);

            if (totalObj != null && totalObj.ContainsKey("TotalLibros"))
            {
                lblTotalLibrosRol3.Text = totalObj["TotalLibros"].ToString();
            }
            else
            {
                lblTotalLibrosRol3.Text = "0";
            }
        }

        
        /// Consume api/dashboard/ultimos-libros y llena el GridView del dashboard Ejecutivo.
        
        private void CargarUltimosLibrosEjecutivo(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/ultimos-libros").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudieron cargar los últimos libros.");

            string json = response.Content.ReadAsStringAsync().Result;
            List<DashboardLibroReciente> libros =
                serializer.Deserialize<List<DashboardLibroReciente>>(json);

            gvUltimosLibrosRol3.DataSource = libros;
            gvUltimosLibrosRol3.DataBind();
        }

        
        /// Consume api/dashboard/libros-por-categoria y llena el repetidor del dashboard Ejecutivo.
        
        private void CargarLibrosPorCategoriaEjecutivo(HttpClient client, JavaScriptSerializer serializer)
        {
            HttpResponseMessage response = client.GetAsync("api/dashboard/libros-por-categoria").Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudieron cargar los libros por categoría.");

            string json = response.Content.ReadAsStringAsync().Result;
            List<DashboardLibroCategoria> categorias =
                serializer.Deserialize<List<DashboardLibroCategoria>>(json);

            rptLibrosCategoriaRol3.DataSource = categorias;
            rptLibrosCategoriaRol3.DataBind();
        }
    }
}