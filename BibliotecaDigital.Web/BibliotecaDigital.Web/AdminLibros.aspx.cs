using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace BibliotecaDigital.Web
{
    
    /// Página de administración de libros.
    /// Consume la API del backend para listar, registrar, editar y eliminar libros.
    /// También permite crear una nueva categoría al momento de guardar o actualizar.
    
    public partial class AdminLibros : System.Web.UI.Page
    {
        
        /// Evento de carga de la página.
        /// Valida sesión y permisos, y carga datos iniciales.
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int idRol = ObtenerIdRolDesdeSesion();

            if (idRol != 1 && idRol != 2 && idRol != 3)
            {
                Response.Redirect("Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarCategorias();
                CargarLibros();
            }
        }

        
        /// Carga las categorías desde la API y agrega la opción de nueva categoría.
        
        private void CargarCategorias()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/categorias").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudieron cargar las categorías.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Categoria> categorias = serializer.Deserialize<List<Categoria>>(json);

                    ddlCategoria.DataSource = categorias;
                    ddlCategoria.DataTextField = "Nombre";
                    ddlCategoria.DataValueField = "IdCategoria";
                    ddlCategoria.DataBind();

                    ddlCategoria.Items.Insert(0, new ListItem("Seleccione...", ""));
                    ddlCategoria.Items.Add(new ListItem("Nueva categoría...", "-1"));
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar categorías: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        
        /// Carga todos los libros desde la API.
        
        private void CargarLibros()
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/libros").Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudieron cargar los libros.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Libro> libros = serializer.Deserialize<List<Libro>>(json);

                    gvLibros.DataSource = libros;
                    gvLibros.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar libros: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }
        
        /// Crea una nueva categoría en la API.
        /// Si la categoría ya existe, reutiliza su Id.
        
        private int CrearNuevaCategoria()
        {
            if (string.IsNullOrWhiteSpace(txtNuevaCategoria.Text))
                throw new Exception("Debe ingresar el nombre de la nueva categoría.");

            if (string.IsNullOrWhiteSpace(txtNuevaRutaFisica.Text))
                throw new Exception("Debe ingresar la ruta física de la nueva categoría.");

            string nombreCategoria = txtNuevaCategoria.Text.Trim();

            using (HttpClient client = ApiHelper.CrearCliente())
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // 1. Verificar si ya existe la categoría
                string urlBusqueda = "api/categorias/por-nombre?nombre=" + Uri.EscapeDataString(nombreCategoria);
                HttpResponseMessage responseBusqueda = client.GetAsync(urlBusqueda).Result;

                if (responseBusqueda.IsSuccessStatusCode)
                {
                    string jsonExistente = responseBusqueda.Content.ReadAsStringAsync().Result;
                    Categoria categoriaExistente = serializer.Deserialize<Categoria>(jsonExistente);

                    if (categoriaExistente != null)
                    {
                        return categoriaExistente.IdCategoria;
                    }
                }

                // 2. Si no existe, crearla
                var requestData = new
                {
                    Nombre = nombreCategoria,
                    RutaFisica = txtNuevaRutaFisica.Text.Trim()
                };

                string json = serializer.Serialize(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage responseInsert = client.PostAsync("api/categorias", content).Result;

                if (!responseInsert.IsSuccessStatusCode)
                {
                    string error = responseInsert.Content.ReadAsStringAsync().Result;
                    throw new Exception("No se pudo crear la categoría: " + error);
                }

                string responseJson = responseInsert.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = serializer.Deserialize<Dictionary<string, object>>(responseJson);

                if (result.ContainsKey("idCategoria"))
                {
                    return Convert.ToInt32(result["idCategoria"]);
                }

                throw new Exception("La API no devolvió el Id de la categoría.");
            }
        }


        /// Obtiene la fecha de publicación validada.
        /// Bonus:
        /// - Permite fecha vacía.
        /// - Bloquea fechas futuras.

        private DateTime? ObtenerFechaPublicacionValidada()
        {
            if (string.IsNullOrWhiteSpace(txtFechaPublicacion.Text))
                return null;

            DateTime fecha = Convert.ToDateTime(txtFechaPublicacion.Text);

            if (fecha.Date > DateTime.Today)
                throw new Exception("La fecha de publicación no puede ser futura.");

            return fecha;
        }

       
        /// Determina qué categoría usar:
        /// una existente o una nueva creada en el momento.
       
        private int ObtenerIdCategoriaFinal()
        {
            if (ddlCategoria.SelectedValue == "-1")
            {
                return CrearNuevaCategoria();
            }

            if (string.IsNullOrWhiteSpace(ddlCategoria.SelectedValue))
            {
                throw new Exception("Debe seleccionar una categoría.");
            }

            return Convert.ToInt32(ddlCategoria.SelectedValue);
        }

        
        /// Registra un libro enviando multipart/form-data a la API.
        
        protected void btnSubir_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            lblMensaje.CssClass = "d-block mt-2 text-danger";

            if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                lblMensaje.Text = "Debe completar título y autor.";
                return;
            }

            if (!fuLibro.HasFile)
            {
                lblMensaje.Text = "Debe seleccionar un archivo.";
                return;
            }

            try
            {
                int idCategoriaFinal = ObtenerIdCategoriaFinal();
                DateTime? fechaPublicacion = ObtenerFechaPublicacionValidada();

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();

                    form.Add(new StringContent(txtTitulo.Text.Trim()), "Titulo");
                    form.Add(new StringContent(txtAutor.Text.Trim()), "Autor");
                    form.Add(new StringContent(txtISBN.Text.Trim()), "ISBN");
                    form.Add(new StringContent(fechaPublicacion.HasValue
                        ? fechaPublicacion.Value.ToString("yyyy-MM-dd")
                        : string.Empty), "FechaPublicacion");
                    form.Add(new StringContent(idCategoriaFinal.ToString()), "IdCategoria");

                    byte[] archivoBytes = fuLibro.FileBytes;
                    ByteArrayContent archivoContent = new ByteArrayContent(archivoBytes);
                    archivoContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    form.Add(archivoContent, "file", fuLibro.FileName);

                    HttpResponseMessage response = client.PostAsync("api/libros/upload", form).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "Libro cargado correctamente.";
                        lblMensaje.CssClass = "d-block mt-2 text-success";

                        LimpiarFormulario();
                        CargarCategorias();
                        CargarLibros();
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        lblMensaje.Text = "No se pudo cargar el libro: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar libro: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        
        /// Maneja las acciones del GridView.
       
        protected void gvLibros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarLibro")
            {
                int idLibro = Convert.ToInt32(e.CommandArgument);
                CargarLibroParaEdicion(idLibro);
            }
            else if (e.CommandName == "EliminarLibro")
            {
                int idLibro = Convert.ToInt32(e.CommandArgument);
                EliminarLibro(idLibro);
            }
        }

        
        /// Carga un libro desde la API en el formulario para edición.
        
        private void CargarLibroParaEdicion(int idLibro)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.GetAsync("api/libros/" + idLibro).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "No se pudo cargar el libro.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Libro libro = serializer.Deserialize<Libro>(json);

                    if (libro == null)
                    {
                        lblMensaje.Text = "No se encontró el libro seleccionado.";
                        lblMensaje.CssClass = "d-block mt-2 text-danger";
                        return;
                    }

                    hfIdLibro.Value = libro.IdLibro.ToString();
                    txtTitulo.Text = libro.Titulo;
                    txtAutor.Text = libro.Autor;
                    txtISBN.Text = libro.ISBN;

                    // Corrección: FechaPublicacion es DateTime?, no string
                    txtFechaPublicacion.Text = libro.FechaPublicacion.HasValue
                        ? libro.FechaPublicacion.Value.ToString("yyyy-MM-dd")
                        : string.Empty;

                    ddlCategoria.SelectedValue = libro.IdCategoria.ToString();

                    btnSubir.Visible = false;
                    btnActualizar.Visible = true;
                    btnCancelar.Visible = true;

                    lblMensaje.Text = "Libro cargado para edición.";
                    lblMensaje.CssClass = "d-block mt-2 text-primary";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar libro: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

       
        /// Actualiza un libro mediante la API.
        /// Puede usar una categoría existente o una nueva.
        
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            lblMensaje.CssClass = "d-block mt-2 text-danger";

            if (string.IsNullOrWhiteSpace(hfIdLibro.Value))
            {
                lblMensaje.Text = "No hay un libro seleccionado para editar.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                lblMensaje.Text = "Debe completar título y autor.";
                return;
            }

            try
            {
                int idCategoriaFinal = ObtenerIdCategoriaFinal();
                DateTime? fechaPublicacion = ObtenerFechaPublicacionValidada();

                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();

                    form.Add(new StringContent(txtTitulo.Text.Trim()), "Titulo");
                    form.Add(new StringContent(txtAutor.Text.Trim()), "Autor");
                    form.Add(new StringContent(txtISBN.Text.Trim()), "ISBN");
                    form.Add(new StringContent(fechaPublicacion.HasValue
                        ? fechaPublicacion.Value.ToString("yyyy-MM-dd")
                        : string.Empty), "FechaPublicacion");
                    form.Add(new StringContent(idCategoriaFinal.ToString()), "IdCategoria");

                    if (fuLibro.HasFile)
                    {
                        byte[] archivoBytes = fuLibro.FileBytes;
                        ByteArrayContent archivoContent = new ByteArrayContent(archivoBytes);
                        archivoContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                        form.Add(archivoContent, "file", fuLibro.FileName);
                    }

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/libros/" + hfIdLibro.Value);
                    request.Content = form;

                    HttpResponseMessage response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "Libro actualizado correctamente.";
                        lblMensaje.CssClass = "d-block mt-2 text-success";

                        LimpiarFormulario();
                        CargarCategorias();
                        CargarLibros();
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        lblMensaje.Text = "No se pudo actualizar el libro: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al actualizar el libro: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

        
        /// Elimina un libro a través de la API.
       
        private void EliminarLibro(int idLibro)
        {
            try
            {
                using (HttpClient client = ApiHelper.CrearCliente())
                {
                    HttpResponseMessage response = client.DeleteAsync("api/libros/" + idLibro).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        lblMensaje.Text = "Libro eliminado correctamente.";
                        lblMensaje.CssClass = "d-block mt-2 text-success";

                        if (hfIdLibro.Value == idLibro.ToString())
                            LimpiarFormulario();

                        CargarLibros();
                    }
                    else
                    {
                        string error = response.Content.ReadAsStringAsync().Result;
                        lblMensaje.Text = "No se pudo eliminar el libro: " + error;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al eliminar el libro: " + ex.Message;
                lblMensaje.CssClass = "d-block mt-2 text-danger";
            }
        }

       
        /// Cancela la edición actual.
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMensaje.Text = "Edición cancelada.";
            lblMensaje.CssClass = "d-block mt-2 text-secondary";
        }

        
        /// Limpia el formulario y restablece el estado inicial.
       
        private void LimpiarFormulario()
        {
            hfIdLibro.Value = string.Empty;
            txtTitulo.Text = string.Empty;
            txtAutor.Text = string.Empty;
            txtISBN.Text = string.Empty;
            txtFechaPublicacion.Text = string.Empty;
            txtNuevaCategoria.Text = string.Empty;
            txtNuevaRutaFisica.Text = string.Empty;

            if (ddlCategoria.Items.Count > 0)
                ddlCategoria.SelectedIndex = 0;

            btnSubir.Visible = true;
            btnActualizar.Visible = false;
            btnCancelar.Visible = false;
        }

        
        /// Convierte el rol guardado en sesión a un Id lógico.
        
        private int ObtenerIdRolDesdeSesion()
        {
            string rol = Session["Rol"] == null ? "" : Session["Rol"].ToString();

            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase)) return 1;
            if (rol.Equals("Bibliotecario", StringComparison.OrdinalIgnoreCase)) return 2;
            if (rol.Equals("Ejecutivo", StringComparison.OrdinalIgnoreCase)) return 3;
            if (rol.Equals("User", StringComparison.OrdinalIgnoreCase)) return 4;

            return 0;
        }
    }
}