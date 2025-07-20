using BSCEvaluacionTecnica.Shared.DTOs;
using Blazored.SessionStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BSCEvaluacionTecnica.Shared.Extensions
{
    public class AutenticacionExtension : AuthenticationStateProvider
    {
        //Crerar variable de sesión.
        private readonly ISessionStorageService _sessionStorage;
        //Variable de claims para revisar parametros .
        private ClaimsPrincipal _sinInformacion = new ClaimsPrincipal(new ClaimsIdentity());

        //Constructor para servicio de sesión.
        public AutenticacionExtension(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Obtener sesión del usuario.
            var sesionUsuario = await _sessionStorage.ObtenerStorage<SesionDTO>("sesionUsuario");

            //Retornar si no se encuentra información.
            if (sesionUsuario == null)
                return await Task.FromResult(new AuthenticationState(_sinInformacion));

            //Si encuentra información generar claim con información.
            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, sesionUsuario.Id.ToString()),
                new Claim("Nombre", sesionUsuario.Nombre),
                new Claim(ClaimTypes.Role, sesionUsuario.Rol),
                new Claim("Correo", sesionUsuario.Correo),
                new Claim("Acceso", sesionUsuario.Acceso),
                new Claim("Token", sesionUsuario.Token)
            }, "JwtAuth"));

            //Retornar el estado de la autenticación con la información obtenida
            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        public async Task ActualizarEstadoAutenticacion(SesionDTO? sesionUsuario)
        {
            ClaimsPrincipal claimsPrincipal;

            //Recibir sesión de usuario.
            if (sesionUsuario != null)
            {
                //Crear claims de información de sesión recibida.
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, sesionUsuario.Id.ToString()),
                    new Claim("Nombre", sesionUsuario.Nombre),
                    new Claim(ClaimTypes.Role, sesionUsuario.Rol),
                    new Claim("Correo", sesionUsuario.Correo),
                    new Claim("Acceso", sesionUsuario.Acceso),
                    new Claim("Token", sesionUsuario.Token)

                }, "JwtAuth"));
                //Guardar en storage la sesion.
                await _sessionStorage.GuardarStorage("sesionUsuario", sesionUsuario);
            }
            else
            {
                //Si la sesión es nula.
                claimsPrincipal = _sinInformacion;
                await _sessionStorage.RemoveItemAsync("sesionUsuario");
            }

            //Notificar que el estado ha cambiado.
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

        }
    }
}
