using BibliotecaDigital.API.Entidades;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BibliotecaDigital.API.Helpers
{
   
    /// Clase auxiliar encargada de generar tokens JWT.
    
    /// JWT (JSON Web Token) permite autenticar usuarios 
    /// de forma segura sin depender únicamente de sesiones tradicionales.
    /// El token contiene información del usuario (claims)
    /// y es firmado digitalmente para evitar manipulaciones.
    
    public static class JwtHelper
    {
        
        /// Genera un token JWT para el usuario autenticado.
        
        /// <param name="usuario">Usuario autenticado desde base de datos.</param>
        /// <returns>Token JWT serializado en formato string.</returns>
        public static string GenerarToken(Usuario usuario)
        {
            // =========================================================
            // 1. OBTENER CONFIGURACIÓN DESDE WEB.CONFIG
            // =========================================================

            // Clave secreta utilizada para firmar el token.
            // Debe mantenerse privada.
            string key = ConfigurationManager.AppSettings["JwtKey"];

            // Nombre del emisor del token.
            // Representa quién genera el token.
            string issuer = ConfigurationManager.AppSettings["JwtIssuer"];

            // Aplicación o cliente autorizado para consumir el token.
            string audience = ConfigurationManager.AppSettings["JwtAudience"];

            // Tiempo de expiración del token en minutos.
            int expireMinutes = int.Parse(
                ConfigurationManager.AppSettings["JwtExpireMinutes"]
            );

            // =========================================================
            // 2. CREAR CLAVE DE SEGURIDAD
            // =========================================================

            // Convierte la clave secreta a bytes UTF8
            // para ser utilizada por el algoritmo de firma.
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            // =========================================================
            // 3. DEFINIR ALGORITMO DE FIRMA
            // =========================================================

            // Se utiliza HMAC SHA256 para firmar el token.
            // Esto garantiza que el token no pueda ser modificado.
            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            // =========================================================
            // 4. CREAR CLAIMS (DATOS DEL USUARIO)
            // =========================================================

            // Los claims representan información del usuario
            // que viajará dentro del token.
            //
            // Estos datos luego podrán ser utilizados
            // por [Authorize] y por User.Identity.
            var claims = new[]
            {
                // Id único del usuario
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.IdUsuario.ToString()
                ),

                // Correo del usuario autenticado.
                // Se usa para identificar quién realiza acciones.
                //
                // IMPORTANTE:
                // User.Identity.Name utilizará este valor.
                new Claim(
                    ClaimTypes.Name,
                    usuario.Correo
                ),

                // Correo electrónico
                new Claim(
                    ClaimTypes.Email,
                    usuario.Correo
                ),

                // Rol del usuario.
                //
                // Esto permite usar:
                // [Authorize(Roles = "1")]
                new Claim(
                    ClaimTypes.Role,
                    usuario.IdRol.ToString()
                ),

                // Nombre completo del usuario
                new Claim(
                    "Nombre",
                    usuario.Nombre
                ),

                // Nombre textual del rol
                new Claim(
                    "RolNombre",
                    usuario.Rol
                )
            };

            // =========================================================
            // 5. CREAR TOKEN JWT
            // =========================================================

            var token = new JwtSecurityToken(

                // Quién emite el token
                issuer: issuer,

                // Quién puede consumir el token
                audience: audience,

                // Información del usuario
                claims: claims,

                // Fecha de expiración
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),

                // Firma digital del token
                signingCredentials: credentials
            );

            // =========================================================
            // 6. SERIALIZAR TOKEN A STRING
            // =========================================================

            // Convierte el objeto JWT a string
            // para enviarlo al frontend.
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}