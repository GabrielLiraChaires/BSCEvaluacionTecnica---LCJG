using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using BSCEvaluacionTecnica.Shared.DTOs;

namespace BSCEvaluacionTecnica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFsController : ControllerBase
    {
        private readonly IConverter _converter;
        public PDFsController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpPost("PDF")]
        public IActionResult Generar([FromBody] PDF parametros)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = parametros.Orientacion == "Horizontal" ? Orientation.Landscape : Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings
                    {
                        Top = parametros.Margenes[0],
                        Right = parametros.Margenes[1],
                        Bottom = parametros.Margenes[2],
                        Left = parametros.Margenes[3]
                    }
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = parametros.Html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            byte[] pdf = _converter.Convert(doc);
            return File(pdf, "application/pdf");
        }
    }
}
