using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace BibliotecaDigital.Web
{
    
    /// Página de administración de usuarios.
    /// Permite listar, crear, editar y eliminar usuarios consumiendo la API.
    /// Acceso permitido:
    /// - Administrador
    /// Regla especial:
    /// - Solo superadmin@bibliomail.com puede eliminar usuarios administradores.
    
    public partial class AdminUsuarios : System.Web.UI.Page
    {
        
        /// Valida sesión, permisos y carga la información inicial.
        
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
                btnGuardarCambios.Visible = false;
                btnCancelar.Visible = false;

                CargarRoles();
                CargarRolesNuevoUsuario();
                CargarUsuarios();
            }
        }

        
        /// Verifica si el usuario actual tiene rol Administrador.
        
        private bool UsuarioEsAdministrador()
        {
            string rol = Session["Rol"] != null ? Session["Rol"].ToString() : string.Empty;
            return rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        }

        
        /// Verifica si el usuario actual es el superadministrador principal del sistema.
        
        private bool UsuarioActualEsSuperAdmin()
        {
            string correo = Session["Correo"] != null ? Session["Correo"].ToString() : string.Empty;
            return correo.Equals("superadmin@bibliomail.com", StringComparison.OrdinalIgnoreCase);
        }

        
        /// Carga los roles disponibles para el formulario de edición.
        
        private void CargarRoles()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/roles").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        MostrarMensaje("No se pudieron cargar los roles.", true);
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
                MostrarMensaje("Error al cargar roles: " + ex.Message, true);
            }
        }

        
        /// Carga los roles disponibles para el formulario de nuevo usuario.
        
        private void CargarRolesNuevoUsuario()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/roles").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        MostrarMensajeNuevoUsuario("No se pudieron cargar los roles.", true);
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
                MostrarMensajeNuevoUsuario("Error al cargar roles: " + ex.Message, true);
            }
        }

        
        /// Carga todos los usuarios desde la API.
       
        private void CargarUsuarios()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/usuarios").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        MostrarMensaje("No se pudieron cargar los usuarios.", true);
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
                MostrarMensaje("Error al cargar usuarios: " + ex.Message, true);
            }
        }

        
        /// Ejecuta acciones del GridView: editar o eliminar usuario.
        
        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarUsuario")
            {
                CargarUsuarioParaEdicion(idUsuario);
            }
            else if (e.CommandName == "EliminarUsuario")
            {
                EliminarUsuario(idUsuario);
            }
        }

        
        /// Carga un usuario seleccionado en el formulario de edición.
        
        private void CargarUsuarioParaEdicion(int idUsuario)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/usuarios/" + idUsuario).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        MostrarMensaje("No se pudo cargar el usuario.", true);
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Usuario usuario = serializer.Deserialize<Usuario>(json);

                    if (usuario == null)
                    {
                        MostrarMensaje("No se encontró el usuario seleccionado.", true);
                        return;
                    }

                    hfIdUsuario.Value = usuario.IdUsuario.ToString();
                    txtNombre.Text = usuario.Nombre;
                    txtCorreo.Text = usuario.Correo;

                    if (ddlRol.Items.FindByValue(usuario.IdRol.ToString()) != null)
                        ddlRol.SelectedValue = usuario.IdRol.ToString();

                    btnGuardarCambios.Visible = true;
                    btnCancelar.Visible = true;

                    MostrarMensaje("Usuario cargado para edición.", false, "primary");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar usuario: " + ex.Message, true);
            }
        }

        
        /// Actualiza los datos principales de un usuario.
        
        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(hfIdUsuario.Value))
            {
                MostrarMensaje("No hay un usuario seleccionado.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(ddlRol.SelectedValue))
            {
                MostrarMensaje("Complete nombre, correo y rol.", true);
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
                        MostrarMensaje("Usuario actualizado correctamente.", false, "success");
                        LimpiarFormularioEdicion();
                        CargarUsuarios();
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        MostrarMensaje("No se pudo actualizar el usuario: " + error, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al actualizar el usuario: " + ex.Message, true);
            }
        }

        
        /// Agrega un nuevo usuario mediante la API.
        
        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            lblMensajeNuevoUsuario.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNuevoNombre.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoPassword.Text) ||
                string.IsNullOrWhiteSpace(ddlNuevoRol.SelectedValue))
            {
                MostrarMensajeNuevoUsuario("Complete nombre, correo, contraseña y rol.", true);
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
                        MostrarMensajeNuevoUsuario("Usuario agregado correctamente.", false, "success");
                        LimpiarFormularioNuevoUsuario();
                        CargarUsuarios();
                    }
                    else
                    {
                        MostrarMensajeNuevoUsuario("No se pudo agregar el usuario: " + responseJson, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeNuevoUsuario("Error al agregar usuario: " + ex.Message, true);
            }
        }

       
        /// Elimina un usuario desde la API.
        /// Antes de eliminar valida si el usuario objetivo es Administrador.
        
        private void EliminarUsuario(int idUsuario)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage getResponse = client.GetAsync("api/usuarios/" + idUsuario).Result;

                    if (!getResponse.IsSuccessStatusCode)
                    {
                        MostrarMensaje("No se pudo cargar el usuario a eliminar.", true);
                        return;
                    }

                    string jsonUsuario = getResponse.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Usuario usuario = serializer.Deserialize<Usuario>(jsonUsuario);

                    if (usuario == null)
                    {
                        MostrarMensaje("No se encontró el usuario a eliminar.", true);
                        return;
                    }

                    if (usuario.IdRol == 1 && !UsuarioActualEsSuperAdmin())
                    {
                        MostrarMensaje("Solo superadmin@bibliomail.com puede eliminar a otro Administrador.", true);
                        return;
                    }

                    HttpResponseMessage deleteResponse = client.DeleteAsync("api/usuarios/" + idUsuario).Result;

                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        MostrarMensaje("Usuario eliminado correctamente.", false, "success");

                        if (hfIdUsuario.Value == idUsuario.ToString())
                            LimpiarFormularioEdicion();

                        CargarUsuarios();
                    }
                    else
                    {
                        string error = deleteResponse.Content.ReadAsStringAsync().Result;
                        MostrarMensaje("No se pudo eliminar el usuario: " + error, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar el usuario: " + ex.Message, true);
            }
        }

        
        /// Cancela la edición actual.
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormularioEdicion();
            MostrarMensaje("Edición cancelada.", false, "secondary");
        }

        
        /// Limpia el formulario de edición.
        
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

        
        /// Limpia el formulario de nuevo usuario.
        
        private void LimpiarFormularioNuevoUsuario()
        {
            txtNuevoNombre.Text = string.Empty;
            txtNuevoCorreo.Text = string.Empty;
            txtNuevoPassword.Text = string.Empty;

            if (ddlNuevoRol.Items.Count > 0)
                ddlNuevoRol.SelectedIndex = 0;
        }

        
        /// Muestra mensajes generales de edición/listado.
        
        private void MostrarMensaje(string mensaje, bool esError, string tipo = null)
        {
            lblMensaje.Text = mensaje;

            if (tipo == null)
                tipo = esError ? "danger" : "success";

            lblMensaje.CssClass = "d-block mt-2 text-" + tipo;
        }

        
        /// Muestra mensajes del formulario de nuevo usuario.
        
        private void MostrarMensajeNuevoUsuario(string mensaje, bool esError, string tipo = null)
        {
            lblMensajeNuevoUsuario.Text = mensaje;

            if (tipo == null)
                tipo = esError ? "danger" : "success";

            lblMensajeNuevoUsuario.CssClass = "d-block mt-2 text-" + tipo;
        }
    }
}