using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace BibliotecaDigital.Web
{
    
    /// Página de administración de usuarios.
    /// Solo el rol Administrador puede acceder.
   
    public partial class AdminUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!UsuarioEsAdministrador())
            {
                Response.Redirect("Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                pnlNuevoUsuario.Visible = true;

                CargarRoles();
                CargarRolesNuevoUsuario();
                CargarUsuarios();
            }
        }

        private bool UsuarioEsAdministrador()
        {
            string rol = Session["Rol"] != null ? Session["Rol"].ToString() : string.Empty;
            return rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }

        private bool UsuarioActualEsSuperAdmin()
        {
            string correo = Session["Correo"] != null ? Session["Correo"].ToString() : string.Empty;
            return correo.Equals("superadmin@bibliomail.com", StringComparison.OrdinalIgnoreCase);
        }

        private void CargarRoles()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/roles").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudieron cargar los roles.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<RolItem> roles = serializer.Deserialize<List<RolItem>>(json);

                    ddlRol.DataSource = roles;
                    ddlRol.DataTextField = "Nombre";
                    ddlRol.DataValueField = "IdRol";
                    ddlRol.DataBind();

                    ddlRol.Items.Insert(0, new ListItem("Seleccione...", ""));
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar roles: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        private void CargarRolesNuevoUsuario()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/roles").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensajeNuevoUsuario.Text = "No se pudieron cargar los roles.";
                        lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<RolItem> roles = serializer.Deserialize<List<RolItem>>(json);

                    ddlNuevoRol.DataSource = roles;
                    ddlNuevoRol.DataTextField = "Nombre";
                    ddlNuevoRol.DataValueField = "IdRol";
                    ddlNuevoRol.DataBind();

                    ddlNuevoRol.Items.Insert(0, new ListItem("Seleccione...", ""));
                }
            }
            catch (Exception ex)
            {
                lblMensajeNuevoUsuario.Text = "Error al cargar roles: " + ex.Message;
                lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-danger";
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/usuarios").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudieron cargar los usuarios.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Usuario> usuarios = serializer.Deserialize<List<Usuario>>(json);

                    gvUsuarios.DataSource = usuarios;
                    gvUsuarios.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar usuarios: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarUsuario")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                CargarUsuarioParaEdicion(idUsuario);
            }
            else if (e.CommandName == "EliminarUsuario")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                EliminarUsuario(idUsuario);
            }
        }

        private void CargarUsuarioParaEdicion(int idUsuario)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/usuarios/" + idUsuario).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudo cargar el usuario.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Usuario usuario = serializer.Deserialize<Usuario>(json);

                    if (usuario == null)
                    {
                        lblMensaje.Text = "No se encontró el usuario seleccionado.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    hfIdUsuario.Value = usuario.IdUsuario.ToString();
                    txtNombre.Text = usuario.Nombre;
                    txtCorreo.Text = usuario.Correo;
                    ddlRol.SelectedValue = usuario.IdRol.ToString();

                    btnGuardarCambios.Visible = true;
                    btnCancelar.Visible = true;

                    lblMensaje.Text = "Usuario cargado para edición.";
                    lblMensaje.CssClass = "d-block mt-2 text-primary";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar usuario: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            lblMensaje.CssClass = "d-block mt-2 text-danger";

            if (string.IsNullOrWhiteSpace(hfIdUsuario.Value))
            {
                lblMensaje.Text = "No hay un usuario seleccionado.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(ddlRol.SelectedValue))
            {
                lblMensaje.Text = "Complete nombre, correo y rol.";
                return;
            }

            try
            {
                int idUsuario = Convert.ToInt32(hfIdUsuario.Value);
                int idRol = Convert.ToInt32(ddlRol.SelectedValue);

                var requestData = new
                {
                    Nombre = txtNombre.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    IdRol = idRol
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(requestData);

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/usuarios/" + idUsuario);
                    request.Content = content;

                    HttpResponseMessage response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "Usuario actualizado correctamente.";
                        lblMensaje.CssClass = "d-block mt-2 text-success";

                        LimpiarFormularioEdicion();
                        CargarUsuarios();
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        lblMensaje.Text = "No se pudo actualizar el usuario: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al actualizar el usuario: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            lblMensajeNuevoUsuario.Text = string.Empty;
            lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-danger";

            if (string.IsNullOrWhiteSpace(txtNuevoNombre.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoPassword.Text) ||
                string.IsNullOrWhiteSpace(ddlNuevoRol.SelectedValue))
            {
                lblMensajeNuevoUsuario.Text = "Complete nombre, correo, contraseña y rol.";
                return;
            }

            try
            {
                var requestData = new
                {
                    Nombre = txtNuevoNombre.Text.Trim(),
                    Correo = txtNuevoCorreo.Text.Trim(),
                    Password = txtNuevoPassword.Text.Trim(),
                    IdRol = Convert.ToInt32(ddlNuevoRol.SelectedValue)
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(requestData);

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync("api/usuarios", content).Result;

                    string responseJson = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblMensajeNuevoUsuario.Text = "Usuario agregado correctamente.";
                        lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-success";

                        LimpiarFormularioNuevoUsuario();
                        CargarUsuarios();
                    }
                    else
                    {
                        lblMensajeNuevoUsuario.Text = "No se pudo agregar el usuario: " + responseJson;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensajeNuevoUsuario.Text = "Error al agregar usuario: " + ex.Message;
                lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-danger";
            }
        }

        private void EliminarUsuario(int idUsuario)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage getResponse = client.GetAsync("api/usuarios/" + idUsuario).Result;

                    if (!getResponse.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudo cargar el usuario a eliminar.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string jsonUsuario = getResponse.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Usuario usuario = serializer.Deserialize<Usuario>(jsonUsuario);

                    if (usuario == null)
                    {
                        lblMensaje.Text = "No se encontró el usuario a eliminar.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    if (usuario.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) &&
                        !UsuarioActualEsSuperAdmin())
                    {
                        lblMensaje.Text = "Solo superadmin@bibliomail.com puede eliminar a otro Administrador.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    HttpResponseMessage deleteResponse = client.DeleteAsync("api/usuarios/" + idUsuario).Result;

                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "Usuario eliminado correctamente.";
                        lblMensaje.CssClass = "d-block mt-2 text-success";

                        if (hfIdUsuario.Value == idUsuario.ToString())
                            LimpiarFormularioEdicion();

                        CargarUsuarios();
                    }
                    else
                    {
                        string error = deleteResponse.Content.ReadAsStringAsync().Result;
                        lblMensaje.Text = "No se pudo eliminar el usuario: " + error;
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al eliminar el usuario: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormularioEdicion();
            lblMensaje.Text = "Edición cancelada.";
            lblMensaje.CssClass = "d-block mt-2 text-secondary";
        }

        private void LimpiarFormularioEdicion()
        {
            hfIdUsuario.Value = string.Empty;
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            if (ddlRol.Items.Count > 0)
                ddlRol.SelectedIndex = 0;

            btnGuardarCambios.Visible = false;
            btnCancelar.Visible = false;
        }

        private void LimpiarFormularioNuevoUsuario()
        {
            txtNuevoNombre.Text = string.Empty;
            txtNuevoCorreo.Text = string.Empty;
            txtNuevoPassword.Text = string.Empty;

            if (ddlNuevoRol.Items.Count > 0)
                ddlNuevoRol.SelectedIndex = 0;
        }
    }
}