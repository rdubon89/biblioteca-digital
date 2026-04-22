using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using BibliotecaDigital.API.Entidades;
using BibliotecaDigital.API.Negocio;

namespace BibliotecaDigital.API.Controllers
{
    
    /// Controlador de libros.
    /// Expone consulta, apertura, descarga, carga, actualización y eliminación.
    
    [RoutePrefix("api/libros")]
    public class LibrosController : ApiController
    {
        
        /// Obtiene todos los libros registrados.
       
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetLibros()
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                var libros = negocio.ObtenerTodos();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

       
        /// Obtiene un libro específico por su Id.
       
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetLibroPorId(int id)
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                Libro libro = negocio.ObtenerPorId(id);

                if (libro == null)
                    return NotFound();

                return Ok(libro);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Devuelve el archivo para abrirlo en el navegador.
        
        [HttpGet]
        [Route("abrir/{id:int}")]
        public IHttpActionResult Abrir(int id)
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                Libro libro = negocio.ObtenerPorId(id);

                if (libro == null || string.IsNullOrWhiteSpace(libro.RutaArchivo))
                    return NotFound();

                string rutaArchivo = libro.RutaArchivo;

                if (!File.Exists(rutaArchivo))
                    return NotFound();

                string extension = Path.GetExtension(rutaArchivo).ToLower();
                string contentType = ObtenerContentType(extension);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(File.OpenRead(rutaArchivo));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = Path.GetFileName(rutaArchivo)
                };

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

      
        /// Devuelve el archivo como descarga.
        
        [HttpGet]
        [Route("descargar/{id:int}")]
        public IHttpActionResult Descargar(int id)
        {
            try
            {
                LibroNegocio negocio = new LibroNegocio();
                Libro libro = negocio.ObtenerPorId(id);

                if (libro == null || string.IsNullOrWhiteSpace(libro.RutaArchivo))
                    return NotFound();

                string rutaArchivo = libro.RutaArchivo;

                if (!File.Exists(rutaArchivo))
                    return NotFound();

                string extension = Path.GetExtension(rutaArchivo).ToLower();
                string contentType = ObtenerContentType(extension);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(File.OpenRead(rutaArchivo));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Path.GetFileName(rutaArchivo)
                };

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Carga un libro nuevo con archivo incluido.
        /// Recibe multipart/form-data.
        
        [HttpPost]
        [Route("upload")]
        public IHttpActionResult UploadLibro()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                string titulo = request.Form["Titulo"];
                string autor = request.Form["Autor"];
                string isbn = request.Form["ISBN"];
                string fechaTexto = request.Form["FechaPublicacion"];
                string idCategoriaTexto = request.Form["IdCategoria"];

                if (string.IsNullOrWhiteSpace(titulo) ||
                    string.IsNullOrWhiteSpace(autor) ||
                    string.IsNullOrWhiteSpace(idCategoriaTexto))
                {
                    return BadRequest("Debe enviar título, autor y categoría.");
                }

                if (request.Files.Count == 0)
                {
                    return BadRequest("Debe enviar un archivo.");
                }

                HttpPostedFile archivo = request.Files[0];

                string extension = Path.GetExtension(archivo.FileName).ToLower();
                int maxBytes = 40 * 1024 * 1024;

                bool extensionValida =
                    extension == ".txt" ||
                    extension == ".tif" ||
                    extension == ".tiff" ||
                    extension == ".pdf" ||
                    extension == ".doc" ||
                    extension == ".docx";

                if (!extensionValida)
                    return BadRequest("Solo se permiten archivos TXT, TIF, TIFF, PDF, DOC y DOCX.");

                if (archivo.ContentLength > maxBytes)
                    return BadRequest("El archivo no puede exceder los 40 MB.");

                int idCategoria = Convert.ToInt32(idCategoriaTexto);

                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                Categoria categoria = categoriaNegocio.ObtenerPorId(idCategoria);

                if (categoria == null || string.IsNullOrWhiteSpace(categoria.RutaFisica))
                    return BadRequest("La categoría no tiene una ruta física configurada.");

                string carpetaBase = categoria.RutaFisica;

                if (!Directory.Exists(carpetaBase))
                    Directory.CreateDirectory(carpetaBase);

                string nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName).Replace(" ", "_");
                string nombreFinal = $"{DateTime.Now:yyyyMMddHHmmss}_{nombreBase}{extension}";
                string rutaFisica = Path.Combine(carpetaBase, nombreFinal);

                archivo.SaveAs(rutaFisica);

                DateTime? fechaPublicacion = null;
                if (!string.IsNullOrWhiteSpace(fechaTexto))
                    fechaPublicacion = Convert.ToDateTime(fechaTexto);

                LibroNegocio libroNegocio = new LibroNegocio();
                int idLibro = libroNegocio.InsertarLibro(
                    titulo.Trim(),
                    autor.Trim(),
                    string.IsNullOrWhiteSpace(isbn) ? null : isbn.Trim(),
                    fechaPublicacion,
                    rutaFisica,
                    idCategoria
                );

                return Ok(new
                {
                    success = true,
                    message = "Libro cargado correctamente.",
                    idLibro = idLibro
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Actualiza un libro.
        /// Puede recibir archivo nuevo o conservar el actual.
        /// Recibe multipart/form-data.
        
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ActualizarLibro(int id)
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                string titulo = request.Form["Titulo"];
                string autor = request.Form["Autor"];
                string isbn = request.Form["ISBN"];
                string fechaTexto = request.Form["FechaPublicacion"];
                string idCategoriaTexto = request.Form["IdCategoria"];

                if (string.IsNullOrWhiteSpace(titulo) ||
                    string.IsNullOrWhiteSpace(autor) ||
                    string.IsNullOrWhiteSpace(idCategoriaTexto))
                {
                    return BadRequest("Debe enviar título, autor y categoría.");
                }

                int idCategoria = Convert.ToInt32(idCategoriaTexto);

                LibroNegocio libroNegocio = new LibroNegocio();
                Libro libroActual = libroNegocio.ObtenerPorId(id);

                if (libroActual == null)
                    return NotFound();

                string rutaArchivo = libroActual.RutaArchivo;

                if (request.Files.Count > 0)
                {
                    HttpPostedFile archivo = request.Files[0];

                    if (archivo != null && archivo.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(archivo.FileName).ToLower();
                        int maxBytes = 40 * 1024 * 1024;

                        bool extensionValida =
                            extension == ".txt" ||
                            extension == ".tif" ||
                            extension == ".tiff" ||
                            extension == ".pdf" ||
                            extension == ".doc" ||
                            extension == ".docx";

                        if (!extensionValida)
                            return BadRequest("Solo se permiten archivos TXT, TIF, TIFF, PDF, DOC y DOCX.");

                        if (archivo.ContentLength > maxBytes)
                            return BadRequest("El archivo no puede exceder los 40 MB.");

                        CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                        Categoria categoria = categoriaNegocio.ObtenerPorId(idCategoria);

                        if (categoria == null || string.IsNullOrWhiteSpace(categoria.RutaFisica))
                            return BadRequest("La categoría no tiene una ruta física configurada.");

                        string carpetaBase = categoria.RutaFisica;

                        if (!Directory.Exists(carpetaBase))
                            Directory.CreateDirectory(carpetaBase);

                        string nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName).Replace(" ", "_");
                        string nombreFinal = $"{DateTime.Now:yyyyMMddHHmmss}_{nombreBase}{extension}";
                        string nuevaRutaArchivo = Path.Combine(carpetaBase, nombreFinal);

                        archivo.SaveAs(nuevaRutaArchivo);

                        if (!string.IsNullOrWhiteSpace(libroActual.RutaArchivo) && File.Exists(libroActual.RutaArchivo))
                            File.Delete(libroActual.RutaArchivo);

                        rutaArchivo = nuevaRutaArchivo;
                    }
                }

                DateTime? fechaPublicacion = null;
                if (!string.IsNullOrWhiteSpace(fechaTexto))
                    fechaPublicacion = Convert.ToDateTime(fechaTexto);

                libroNegocio.ActualizarLibro(
                    id,
                    titulo.Trim(),
                    autor.Trim(),
                    string.IsNullOrWhiteSpace(isbn) ? null : isbn.Trim(),
                    fechaPublicacion,
                    rutaArchivo,
                    idCategoria
                );

                return Ok(new
                {
                    success = true,
                    message = "Libro actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Elimina un libro y su archivo físico si existe.
        
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult EliminarLibro(int id)
        {
            try
            {
                LibroNegocio libroNegocio = new LibroNegocio();
                Libro libro = libroNegocio.ObtenerPorId(id);

                if (libro == null)
                    return NotFound();

                if (!string.IsNullOrWhiteSpace(libro.RutaArchivo) && File.Exists(libro.RutaArchivo))
                    File.Delete(libro.RutaArchivo);

                libroNegocio.EliminarLibro(id);

                return Ok(new
                {
                    success = true,
                    message = "Libro eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        
        /// Devuelve el Content-Type según la extensión del archivo.
        
        private string ObtenerContentType(string extension)
        {
            if (extension == ".pdf") return "application/pdf";
            if (extension == ".txt") return "text/plain";
            if (extension == ".doc") return "application/msword";
            if (extension == ".docx") return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            if (extension == ".tif" || extension == ".tiff") return "image/tiff";

            return "application/octet-stream";
        }
    }
}