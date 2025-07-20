using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.Server.Custom;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;

        public UsuarioController(BSCEvaluacionTecnicaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar()
        {
            var responseAPI = new ResponseAPI<List<UsuarioDTO>>();
            try
            {
                var listaModulosDTO = await _context.Usuarios.Select(x => new UsuarioDTO
                {
                    Id= x.Id,
                    Nombre= x.Nombre,
                    Correo= x.Correo,
                    Rol=x.Rol
                }).ToListAsync();

                responseAPI.EsCorrecto = true;
                responseAPI.Valor = listaModulosDTO;

                return StatusCode(StatusCodes.Status200OK, responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al cargar módulos. Detalles: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }
    }
}
