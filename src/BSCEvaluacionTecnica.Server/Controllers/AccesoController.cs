using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.Server.Custom;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;
        private readonly Utilidades _utilidades;

        public AccesoController(BSCEvaluacionTecnicaContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var responseAPI = new ResponseAPI<SesionDTO>();

            try
            {
                var clave = _utilidades.EncriptarSHA256(loginDTO.Clave);
                var usuarioEncontrado = await _context.Usuarios.Where(u => u.Correo == loginDTO.Correo && u.Contrasena == clave).FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                {
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = "El usuario y/o contraseña son incorrectos.";
                    return StatusCode(StatusCodes.Status200OK, responseAPI);
                }
                else
                {
                    //Buscar permiso de inicio de sesión.
                    var acceso = await _context.PermisoModuloUsuarios.Include(x => x.FkIdModuloSistemaNavigation).Where(x => x.FkIdUsuario == usuarioEncontrado.Id && x.FkIdModuloSistemaNavigation.Nombre == "InicioSesion").FirstOrDefaultAsync();

                    SesionDTO sesion = new SesionDTO
                    {
                        Id = usuarioEncontrado.Id,
                        Rol = usuarioEncontrado.Rol,
                        Nombre = usuarioEncontrado.Nombre,
                        Correo=usuarioEncontrado.Correo,
                        Acceso = acceso!.Acceso,
                        Token = _utilidades.GenerarJWT(usuarioEncontrado)
                    };
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = sesion;
                    responseAPI.Mensaje = "Inicio de sesión exitoso.";

                    return StatusCode(StatusCodes.Status200OK, responseAPI);
                }
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al iniciar sesión. Detalles: " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }

        [HttpGet]
        [Route("ValidarToken")]
        public async Task<IActionResult> ValidarToken([FromQuery] string token)
        {
            var responseAPI = new ResponseAPI<string>();
            (bool, string) respuesta = _utilidades.ValidarToken(token);

            responseAPI.EsCorrecto = respuesta.Item1;

            if (respuesta.Item1)
                responseAPI.Valor = respuesta.Item2;
            else
                responseAPI.Mensaje = respuesta.Item2;

            return StatusCode(StatusCodes.Status200OK, responseAPI);
        }
    }
}
