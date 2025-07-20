using BSCEvaluacionTecnica.DataAccess.Entities;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BSCEvaluacionTecnica.Server.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuracion;
        public Utilidades(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        //EncriptarClave.
        public string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Computar el hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                //Convertir arreglo de bytes a string.
                StringBuilder constructor = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    constructor.Append(bytes[i].ToString("x2"));
                }
                return constructor.ToString();
            }
        }

        //Generar JWT.
        public string GenerarJWT(Usuario usuario)
        {
            //Crear información de usuario para token.
            var claimUsuario = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim("Nombre", usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("Correo", usuario.Correo),
            };

            var llaveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["JWT:Clave"]!));
            var credenciales = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256Signature);

            //Crear detalle de Token.
            var configuracionJWT = new JwtSecurityToken
            (
                claims: claimUsuario,
                expires: DateTime.Now.AddMinutes(15), //Duración de token.
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(configuracionJWT);
        }

        //Validar Token Enviado.
        public (bool, string) ValidarToken(string token)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false, //Validar verificación para aplicación externa.
                ValidateAudience = false, //Servidor o Dominio que accederá a API.
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuracion["JWT:Clave"]!))
            };
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return (true, "Token válido");
            }
            catch (SecurityTokenExpiredException) //Atrapar token expirado.
            {
                return (false, "Token expirado. Redirigiendo a Login.");
            }
            catch (SecurityTokenInvalidSignatureException) //Atrapar token con firma invalida.
            {
                return (false, "Token con firma inválida. Evite modificar la sesión.");
            }
            catch (SecurityTokenMalformedException)
            {
                return (false, "Token mal generado. Evite modificar la sesión.");
            }
            catch (Exception ex)
            {
                return (false, "Token inválido. Evite modificar la sesión.");
            }
        }
    }
}
