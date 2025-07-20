using BSCEvaluacionTecnica.Business.Interfaces;
using BSCEvaluacionTecnica.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using BSCEvaluacionTecnica.Shared.DTOs;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Services
{
    public class AccesoService : IAccesoService
    {
        private readonly HttpClient _http;
        public AccesoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<SesionDTO> Login(LoginDTO loginDTO)
        {
            var result = await _http.PostAsJsonAsync("api/Acceso/Login", loginDTO);

            // Registrar el resultado HTTP por si algo falla.
            //Console.WriteLine($"Código de estado: {result.StatusCode}");

            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<ResponseAPI<SesionDTO>>();

                // Registrar la respuesta para depurar.
                //Console.WriteLine($"Respuesta: {response}");

                if (response != null && response.EsCorrecto)
                {
                    return response.Valor!;
                }
                else if (response != null && response.EsCorrecto == false)
                {
                    throw new Exception(response.Mensaje);
                }
                else
                {
                    throw new Exception(response?.Mensaje + " Error desconocido en la respuesta.");
                }
            }
            else
            {
                throw new Exception("Error en la solicitud. Código de estado: " + result.StatusCode);
            }
        }

        public async Task<string> ValidarToken(string token)
        {
            var response = await _http.GetFromJsonAsync<ResponseAPI<string>>($"api/Acceso/ValidarToken?token=" + token);

            if (response!.EsCorrecto)
            {
                return response.Valor!;
            }
            else
            {
                return response.Mensaje!;
            }
        }
    }
}
