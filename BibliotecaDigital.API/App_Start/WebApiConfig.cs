using System.Web.Http;

namespace BibliotecaDigital.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Habilita rutas por atributos como [Route]
            config.MapHttpAttributeRoutes();

            // Ruta por defecto
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
