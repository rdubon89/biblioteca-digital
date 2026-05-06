using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BibliotecaDigital.Web
{
    
    /// Clase auxiliar para crear clientes HTTP que consuman la API del backend.
    /// También agrega el token JWT cuando existe en sesión.
    
    public static class ApiHelper
    {
        private static readonly string baseUrl = "https://localhost:44341/";

        public static HttpClient CrearCliente()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Agregar token JWT si existe en sesión
            if (HttpContext.Current != null &&
                HttpContext.Current.Session != null &&
                HttpContext.Current.Session["Token"] != null)
            {
                string token = HttpContext.Current.Session["Token"].ToString();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}