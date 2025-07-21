using BSCEvaluacionTecnica.Business.Interfaces;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BSCEvaluacionTecnica.Business.Services
{
    public class PDFService : IPDFService
    {
        HttpClient _http;
        IJSRuntime _js;
        public PDFService(HttpClient _httpClient, IJSRuntime js)
        {
            _http = _httpClient;
            _js = js;
        }

        public async Task<string> PDF(PDF parametros)
        {
            try
            {
                //Generando request a la API y asignandolo en formato JSON.
                var request = new HttpRequestMessage(HttpMethod.Post, "api/PDFs/PDF");
                request.Content = new StringContent(JsonSerializer.Serialize(parametros), Encoding.UTF8, "application/json");

                //Envio de petición.
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();

                    using var ms = new MemoryStream();
                    await result.CopyToAsync(ms);
                    var bytes = ms.ToArray();

                    // Convertir a Base64.
                    var base64 = Convert.ToBase64String(bytes);
                    var dataUrl = $"data:application/pdf;base64,{base64}";

                    return base64;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
    }
}
