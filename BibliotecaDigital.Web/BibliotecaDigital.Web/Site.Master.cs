using System;

namespace BibliotecaDigital.Web
{
    
    /// Master page principal del sistema.
    /// Controla la navegación global, sesión activa, visibilidad por roles
    /// y cierre de sesión del usuario autenticado.
    
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        
        /// Evento inicial de carga del MasterPage.
        /// Configura el navbar en cada carga para mantenerlo actualizado.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarNavbarPorSesion();
        }

        
        /// Configura los elementos visibles del navbar según la sesión y el rol actual.
        
        private void ConfigurarNavbarPorSesion()
        {
            bool sesionActiva = Session["UsuarioId"] != null;

            string rol = Session["Rol"] != null
                ? Session["Rol"].ToString()
                : string.Empty;

            string nombre = Session["Nombre"] != null
                ? Session["Nombre"].ToString()
                : "Usuario";

            pnlMenuPublico.Visible = !sesionActiva;
            pnlMenuAutenticado.Visible = sesionActiva;

            if (!sesionActiva)
            {
                return;
            }

            lblUsuarioActual.Text = nombre;

            lnkDashboard.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Ejecutivo", StringComparison.OrdinalIgnoreCase);

            lnkLibros.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase);

            lnkUsuarios.Visible =
                rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }

        
        /// Cierra la sesión local del usuario y redirige al Login.
        
        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            Response.Redirect("Login.aspx");
        }
    }
}