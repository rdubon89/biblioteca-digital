using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace BibliotecaDigital.Web
{
    
    /// Página de inicio de sesión del sistema.
    /// Consume la API del backend para validar credenciales.
   
    public partial class Login : System.Web.UI.Page
    {
        
        /// Evento de carga de la página.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // El tema visual se controla desde Site.Master
        }

        
        /// Valida los campos, consume la API de autenticación
        /// y crea la sesión del usuario si las credenciales son correctas.
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;

            // Validar correo
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                lblMensaje.Text = "Debe ingresar el correo.";
                return;
            }

            // Validar contraseña
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMensaje.Text = "Debe ingresar la contraseña.";
                return;
            }

            try
            {
                // Crear objeto con los datos de login
                var requestData = new
                {
                    Correo = txtCorreo.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };

                // Serializar a JSON
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(requestData);

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    // Crear contenido HTTP en formato JSON
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Consumir el endpoint POST /api/auth/login
                    HttpResponseMessage response = client.PostAsync("api/auth/login", content).Result;

                    // Verificar respuesta HTTP
                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudo conectar correctamente con el backend.";
                        return;
                    }

                    // Leer respuesta como texto JSON
                    string responseJson = response.Content.ReadAsStringAsync().Result;

                    // Convertir JSON a objeto
                    LoginApiResponse result = serializer.Deserialize<LoginApiResponse>(responseJson);

                    // Validar respuesta lógica
                    if (result != null && result.success && result.usuario != null)
                    {
                        // Guardar datos en sesión
                        Session["UsuarioId"] = result.usuario.IdUsuario;
                        Session["Nombre"] = result.usuario.Nombre;
                        Session["Rol"] = result.usuario.Rol;
                        Session["IdRol"] = result.usuario.IdRol;
                        Session["Correo"] = result.usuario.Correo;

                        // Redirigir al Home
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        lblMensaje.Text = result != null && !string.IsNullOrWhiteSpace(result.message)
                            ? result.message
                            : "Correo o contraseña incorrectos.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }
    }
}