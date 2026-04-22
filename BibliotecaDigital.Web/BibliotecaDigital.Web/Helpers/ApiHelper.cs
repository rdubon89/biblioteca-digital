using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BibliotecaDigital.Web
{
    /// <summary>
    /// Clase auxiliar para crear clientes HTTP que consuman la API del backend.
    /// </summary>
    public static class ApiHelper
    {
        
        /// URL base de la API.
        /// Debe coincidir con el puerto donde se ejecuta BibliotecaDigital.API.
        
        private static readonly string baseUrl = "https://localhost:44341/";

       
        /// Crea un HttpClient configurado para consumir servicios JSON.
        
        /// <returns>Instancia de HttpClient configurada.</returns>
        public static HttpClient CrearCliente()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);

            // Limpiar encabezados previos
            client.DefaultRequestHeaders.Accept.Clear();

            // Indicar que se espera JSON como respuesta
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}