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
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _autenticacionExtension;
        public UsuarioService(HttpClient http, AuthenticationStateProvider autenticacionExtension)
        {
            _http = http;
            _autenticacionExtension = autenticacionExtension;
        }

        public async Task<List<UsuarioDTO>> Consultar()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Usuario/Consultar");
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<UsuarioDTO>>>();
                if (result!.EsCorrecto)
                {
                    return result.Valor ?? new List<UsuarioDTO>();
                }
                else
                {
                    throw new Exception(result?.Mensaje);
                }
            }
            else
            {
                throw new Exception("Error al obtener los usuarios registrados.");
            }
        }
    }
}
