using BSCEvaluacionTecnica.Business.Interfaces;
using BSCEvaluacionTecnica.Shared.DTOs;
using BSCEvaluacionTecnica.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Services
{
    public class PermisoModulosService : IPermisoModulosService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _autenticacionExtension;
        public PermisoModulosService(HttpClient http, AuthenticationStateProvider autenticacionExtension)
        {
            _http = http;
            _autenticacionExtension = autenticacionExtension;
        }
        public async Task<List<PermisoModuloUsuarioDTO>> ConsultarModulosUsuario(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/PermisoModulos/ConsultarModulosUsuario/{id}");
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<PermisoModuloUsuarioDTO>>>();
                if (result!.EsCorrecto)
                {
                    return result.Valor!;
                }
                else
                {
                    throw new Exception(result?.Mensaje);
                }
            }
            else
            {
                throw new Exception("Error al obtener los permisos del usuario.");
            }
        }
    }
}
