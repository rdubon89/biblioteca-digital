using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace BibliotecaDigital.Web
{
    
    /// Página de registro de usuario.
    /// Consume la API del backend para registrar nuevas cuentas.
    
    public partial class Registro : System.Web.UI.Page
    {
        
        /// Evento de carga de la página.
        
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        
        /// Registra un nuevo usuario consumiendo la API.
       
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            lblMensaje.CssClass = "text-danger mt-3 d-block";

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblMensaje.Text = "Debe ingresar el nombre.";
                return;
            }

            // Validar correo
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                lblMensaje.Text = "Debe ingresar el correo.";
                return;
            }

            // Validación básica de formato de correo
            if (!txtCorreo.Text.Contains("@"))
            {
                lblMensaje.Text = "Debe ingresar un correo válido.";
                return;
            }

            // Validar contraseña
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMensaje.Text = "Debe ingresar la contraseña.";
                return;
            }

            // Validar confirmación de contraseña
            if (string.IsNullOrWhiteSpace(txtConfirmarPassword.Text))
            {
                lblMensaje.Text = "Debe confirmar la contraseña.";
                return;
            }

            // Validar coincidencia
            if (txtPassword.Text.Trim() != txtConfirmarPassword.Text.Trim())
            {
                lblMensaje.Text = "Las contraseñas no coinciden.";
                return;
            }

            try
            {
                // Preparar datos para enviar a la API
                var requestData = new
                {
                    Nombre = txtNombre.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(requestData);

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("api/auth/register", content).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudo conectar correctamente con el backend.";
                        return;
                    }

                    string responseJson = response.Content.ReadAsStringAsync().Result;
                    var result = serializer.Deserialize<dynamic>(responseJson);

                    bool success = result.ContainsKey("success") && Convert.ToBoolean(result["success"]);
                    string message = result.ContainsKey("message")
                        ? result["message"].ToString()
                        : "Error desconocido.";

                    if (success)
                    {
                        lblMensaje.Text = message;
                        lblMensaje.CssClass = "text-success mt-3 d-block";

                        // Limpiar formulario
                        txtNombre.Text = string.Empty;
                        txtCorreo.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        txtConfirmarPassword.Text = string.Empty;
                    }
                    else
                    {
                        lblMensaje.Text = message;
                        lblMensaje.CssClass = "text-danger mt-3 d-block";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al registrar usuario: " + ex.Message;
                lblMensaje.CssClass = "text-danger mt-3 d-block";
            }
        }
    }
}