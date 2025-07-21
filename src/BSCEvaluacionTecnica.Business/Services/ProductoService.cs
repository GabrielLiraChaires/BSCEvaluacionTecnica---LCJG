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
using System.Text.Json;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Services
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _autenticacionExtension;
        public ProductoService(HttpClient http, AuthenticationStateProvider autenticacionExtension)
        {
            _http = http;
            _autenticacionExtension = autenticacionExtension;
        }
        public async Task<List<ProductoDTO>> Consultar()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Producto/Consultar");
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ProductoDTO>>>();
                if (result!.EsCorrecto)
                {
                    return result.Valor ?? new List<ProductoDTO>();
                }
                else
                {
                    throw new Exception(result?.Mensaje);
                }
            }
            else
            {
                throw new Exception("Error al obtener los productos registrados.");
            }
        }

        public async Task<ResponseAPI<ProductoDTO>> Guardar(ProductoDTO usuario)
        {
            //Generando request a la API y asignandolo en formato JSON.
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Producto/Guardar");
            request.Content = new StringContent(JsonSerializer.Serialize(usuario), Encoding.UTF8, "application/json");

            //Tomando el toquen de la sesión.
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            //Asignación de token a headers.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Envio de petición.
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<ProductoDTO>>();
                return result ?? new ResponseAPI<ProductoDTO>();
            }
            else
            {
                throw new Exception("Error al guardar el producto.");
            }
        }

        public async Task<ResponseAPI<ProductoDTO>> Actualizar(ProductoDTO usuario)
        {
            //Generando request a la API y asignandolo en formato JSON.
            var request = new HttpRequestMessage(HttpMethod.Put, "api/Producto/Actualizar");
            request.Content = new StringContent(JsonSerializer.Serialize(usuario), Encoding.UTF8, "application/json");

            //Tomando el toquen de la sesión.
            var sesion = await _autenticacionExtension.GetAuthenticationStateAsync();
            var token = sesion.User.FindFirst("Token")!.Value.ToString();

            //Asignación de token a headers.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Envio de petición.
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<ProductoDTO>>();
                return result ?? new ResponseAPI<ProductoDTO>();
            }
            else
            {
                throw new Exception("Error al actualizar el producto.");
            }
        }
    }
}
