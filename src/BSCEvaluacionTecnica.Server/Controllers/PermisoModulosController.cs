using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.Server.Custom;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisoModulosController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;
        private readonly Utilidades _utilidades;

        public PermisoModulosController(BSCEvaluacionTecnicaContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarModulosUsuario/{id}")]
        public async Task<IActionResult> ConsultarModulosUsuario(int id)
        {
            
            var responseAPI = new ResponseAPI<List<PermisoModuloUsuarioDTO>>();
            try
            {
                var listaModulosDTO = await _context.PermisoModuloUsuarios.Include(u => u.FkIdUsuarioNavigation).Include(m => m.FkIdModuloSistemaNavigation).Where(i => i.FkIdUsuario == id).Select(x => new PermisoModuloUsuarioDTO
                {
                    FkIdUsuario = x.FkIdUsuario,
                    FkIdModuloSistema = x.FkIdModuloSistema,
                    Acceso = x.Acceso,
                    ModuloSistema = x.FkIdModuloSistemaNavigation.Nombre,
                    Correo = x.FkIdUsuarioNavigation.Correo,
                    NombreCompletoUsuario = x.FkIdUsuarioNavigation.Nombre,
                    RolUsuario = x.FkIdUsuarioNavigation.Rol,
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
