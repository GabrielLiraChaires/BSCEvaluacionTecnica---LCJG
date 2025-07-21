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
    public class ProductoController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;
        private readonly Utilidades _utilidades;

        public ProductoController(BSCEvaluacionTecnicaContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpGet]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar()
        {
            var responseAPI = new ResponseAPI<List<ProductoDTO>>();
            try
            {
                var listaproductos = await _context.Productos.Select(x => new ProductoDTO
                {
                    Clave = x.Clave,
                    Nombre = x.Nombre,
                    Existencias = x.Existencias,
                    CostoUnidad = x.CostoUnidad
                }).ToListAsync();

                responseAPI.EsCorrecto = true;
                responseAPI.Valor = listaproductos;

                return StatusCode(StatusCodes.Status200OK, responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al cargar los productos. Detalles: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult<ResponseAPI<ProductoDTO>>> Guardar(ProductoDTO productoDTO)
        {
            var responseAPI = new ResponseAPI<ProductoDTO>();
            //TRANSACCIÓN.
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //Guardando usuario.
                    var producto = new Producto
                    {
                        Clave = productoDTO.Clave!,
                        Nombre = productoDTO.Nombre!,
                        Existencias = productoDTO.Existencias,
                        CostoUnidad = productoDTO.CostoUnidad
                    };
                    await _context.Productos.AddAsync(producto);
                    await _context.SaveChangesAsync();

                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new ProductoDTO
                    {
                        Clave = producto.Clave!,
                        Nombre = producto.Nombre!,
                        Existencias = producto.Existencias,
                        CostoUnidad = producto.CostoUnidad
                    };
                    responseAPI.Mensaje = "Producto guardado exitosamente.";
                    return StatusCode(StatusCodes.Status201Created, responseAPI);
                }
                catch (Exception)
                {
                    //Si fue encontrado un error, se revierte la transacción.
                    await transaccion.RollbackAsync();
                    //Respuesta.
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = $"Ocurrió un error al guardar el producto.";
                    return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
                }
            }
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<ActionResult<ResponseAPI<ProductoDTO>>> Actualizar(ProductoDTO productoDTO)
        {
            var responseAPI = new ResponseAPI<ProductoDTO>();
            //TRANSACCIÓN.
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //Actualizando usuario.
                    var existencia = await _context.Productos.Where(x => x.Clave == productoDTO.Clave).FirstOrDefaultAsync();

                    existencia.Nombre = productoDTO.Nombre!;
                    existencia.Existencias = productoDTO.Existencias;
                    existencia.CostoUnidad = productoDTO.CostoUnidad;

                    _context.Productos.Update(existencia);
                    await _context.SaveChangesAsync();

                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new ProductoDTO
                    {
                        Clave = existencia.Clave,
                        Nombre = existencia.Nombre!,
                        Existencias = existencia.Existencias,
                        CostoUnidad=existencia.CostoUnidad
                    };
                    responseAPI.Mensaje = "Producto actualizado exitosamente.";
                    return StatusCode(StatusCodes.Status201Created, responseAPI);
                }
                catch (Exception)
                {
                    //Si fue encontrado un error, se revierte la transacción.
                    await transaccion.RollbackAsync();
                    //Respuesta.
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = $"Ocurrió un error al actualizar el producto.";
                    return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
                }
            }
        }
    }
}
