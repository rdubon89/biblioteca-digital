using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using System.Text;

[assembly: OwinStartup(typeof(BibliotecaDigital.API.Startup))]

namespace BibliotecaDigital.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string key = ConfigurationManager.AppSettings["JwtKey"];
            string issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            string audience = ConfigurationManager.AppSettings["JwtAudience"];

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    ValidateLifetime = true
                }
            });
        }
    }
}