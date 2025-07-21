using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.DataAccess.Entities;
using BSCEvaluacionTecnica.Server.Custom;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BSCEvaluacionTecnica.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;
        private readonly Utilidades _utilidades;

        public UsuarioController(BSCEvaluacionTecnicaContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpGet]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar()
        {
            var responseAPI = new ResponseAPI<List<UsuarioDTO>>();
            try
            {
                var listaUsuarios = await _context.Usuarios.Select(x => new UsuarioDTO
                {
                    Id= x.Id,
                    Nombre= x.Nombre,
                    Correo= x.Correo,
                    Rol=x.Rol
                }).ToListAsync();

                responseAPI.EsCorrecto = true;
                responseAPI.Valor = listaUsuarios;

                return StatusCode(StatusCodes.Status200OK, responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al cargar los usuarios. Detalles: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult<ResponseAPI<UsuarioDTO>>> Guardar(UsuarioDTO usuarioDTO)
        {
            var responseAPI = new ResponseAPI<UsuarioDTO>();
            //TRANSACCIÓN.
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //Guardando usuario.
                    var usuario = new Usuario
                    {
                        Id = 0, //A la espera de el guardado.
                        Nombre = usuarioDTO.Nombre!,
                        Correo = usuarioDTO.Correo!,
                        Contrasena = _utilidades.EncriptarSHA256(usuarioDTO.Clave!),
                        Rol = usuarioDTO.Rol!
                    };
                    await _context.Usuarios.AddAsync(usuario);
                    await _context.SaveChangesAsync();

                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre!,
                        Correo = usuario.Correo!,
                        Clave = string.Empty,
                        ClaveConfirmacion = string.Empty,
                        Rol = usuario.Rol!
                    };
                    responseAPI.Mensaje = "Usuario guardado exitosamente.";
                    return StatusCode(StatusCodes.Status201Created, responseAPI);
                }
                catch (Exception)
                {
                    //Si fue encontrado un error, se revierte la transacción.
                    await transaccion.RollbackAsync();
                    //Respuesta.
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = $"Ocurrió un error al guardar el usuario.";
                    return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
                }
            }
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<ActionResult<ResponseAPI<UsuarioDTO>>> Actualizar(UsuarioDTO usuarioDTO)
        {
            var responseAPI = new ResponseAPI<UsuarioDTO>();
            //TRANSACCIÓN.
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //Actualizando usuario.
                    var existencia = await _context.Usuarios.Where(x => x.Id == usuarioDTO.Id).FirstOrDefaultAsync();

                    existencia.Nombre = usuarioDTO.Nombre!;
                    existencia.Correo = usuarioDTO.Correo!;
                    existencia.Contrasena = string.IsNullOrWhiteSpace(usuarioDTO.Clave) ? existencia.Contrasena : (_utilidades.EncriptarSHA256(usuarioDTO.Clave!) == existencia.Contrasena ? existencia.Contrasena : _utilidades.EncriptarSHA256(usuarioDTO.Clave!));
                    existencia.Rol = usuarioDTO.Rol!;

                    _context.Usuarios.Update(existencia);
                    await _context.SaveChangesAsync();

                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new UsuarioDTO
                    {
                        Id = existencia.Id,
                        Nombre = existencia.Nombre!,
                        Correo = existencia.Correo!,
                        Clave = string.Empty,
                        ClaveConfirmacion = string.Empty,
                        Rol = existencia.Rol!
                    };
                    responseAPI.Mensaje = "Usuario actualizado exitosamente.";
                    return StatusCode(StatusCodes.Status201Created, responseAPI);
                }
                catch (Exception)
                {
                    //Si fue encontrado un error, se revierte la transacción.
                    await transaccion.RollbackAsync();
                    //Respuesta.
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = $"Ocurrió un error al actualizar el usuario.";
                    return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
                }
            }
        }
    }
}
