using BSCEvaluacionTecnica.Business.Interfaces;
using BSCEvaluacionTecnica.DataAccess.Entities;
using BSCEvaluacionTecnica.Shared.DTOs;
using BSCEvaluacionTecnica.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _autenticacionExtension;
        public PedidosService(HttpClient http, AuthenticationStateProvider autenticacionExtension)
        {
            _http = http;
            _autenticacionExtension = autenticacionExtension;
        }

        public async Task<List<RegistroPedidosDTO>> HistorialPedidos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Pedido/Consultar");
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<RegistroPedidosDTO>>>();
                if (result!.EsCorrecto)
                {
                    return result.Valor ?? new List<RegistroPedidosDTO>();
                }
                else
                {
                    throw new Exception(result?.Mensaje);
                }
            }
            else
            {
                throw new Exception("Error al obtener los pedidos registrados.");
            }
        }

        public async Task<List<RegistroPedidosDTO>> HistorialPedidosVendedor(int vendedor)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Pedido/ConsultarVendedor/{vendedor}");
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<RegistroPedidosDTO>>>();
                if (result!.EsCorrecto)
                {
                    return result.Valor ?? new List<RegistroPedidosDTO>();
                }
                else
                {
                    throw new Exception(result?.Mensaje);
                }
            }
            else
            {
                throw new Exception("Error al obtener los pedidos registrados.");
            }
        }

        public async Task<ResponseAPI<List<RegistroPedidosDTO>>> Guardar(RegistroPedidosDTO pedido)
        {
            //Generando request a la API y asignandolo en formato JSON.
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Pedido/Guardar");
            request.Content = new StringContent(JsonSerializer.Serialize(pedido), Encoding.UTF8, "application/json");

            //Tomando el toquen de la sesión.
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            //Asignación de token a headers.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Envio de petición.
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<RegistroPedidosDTO>>>();
                return result ?? new ResponseAPI<List<RegistroPedidosDTO>>();
            }
            else
            {
                throw new Exception("Error al guardar el pedido.");
            }
        }
    }
}
