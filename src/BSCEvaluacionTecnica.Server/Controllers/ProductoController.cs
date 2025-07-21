using BSCEvaluacionTecnica.DataAccess.Context;
using BSCEvaluacionTecnica.DataAccess.Entities;
using BSCEvaluacionTecnica.Server.Custom;
using BSCEvaluacionTecnica.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
                    //Guardando producto usando entity framework.
                    #region 
                    /*var producto = new Producto
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
                    return StatusCode(StatusCodes.Status201Created, responseAPI);*/
                    #endregion

                    //Guardando producto usando procedimientos almacenados.
                    //Llamando al procedimiento almacenado.
                    var resultado = await _context.Productos.FromSqlInterpolated($"""EXEC sp_GuardarProducto @Clave = {productoDTO.Clave}, @Nombre = {productoDTO.Nombre}, @Existencias = {productoDTO.Existencias}, @CostoUnidad = {productoDTO.CostoUnidad}""").ToListAsync();
                    //Debe devolver exactamente el registro que acaba de insertar.
                    var guardado = resultado.First();
                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new ProductoDTO
                    {
                        Clave = guardado.Clave,
                        Nombre = guardado.Nombre,
                        Existencias = guardado.Existencias,
                        CostoUnidad = guardado.CostoUnidad
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
                    //Actualizando usuario usando entity framework.
                    #region
                    /*var existencia = await _context.Productos.Where(x => x.Clave == productoDTO.Clave).FirstOrDefaultAsync();

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
                    return StatusCode(StatusCodes.Status201Created, responseAPI);*/
                    #endregion
                    //Actualizando producto usando procedimientos almacenados.
                    //Llamando al procedimiento almacenado.
                    var resultado = await _context.Productos.FromSqlInterpolated($"""EXEC sp_ActualizarProducto @Clave = {productoDTO.Clave}, @Nombre = {productoDTO.Nombre}, @Existencias = {productoDTO.Existencias}, @CostoUnidad = {productoDTO.CostoUnidad}""").ToListAsync();
                    //Debe devolver exactamente el registro que acaba de actualizar.
                    var actualizado = resultado.First();
                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = new ProductoDTO
                    {
                        Clave = actualizado.Clave,
                        Nombre = actualizado.Nombre,
                        Existencias = actualizado.Existencias,
                        CostoUnidad = actualizado.CostoUnidad
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
