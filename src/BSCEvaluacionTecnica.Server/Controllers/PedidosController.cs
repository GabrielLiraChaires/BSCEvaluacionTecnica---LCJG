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
    public class PedidoController : Controller
    {
        private readonly BSCEvaluacionTecnicaContext _context;
        private readonly Utilidades _utilidades;

        public PedidoController(BSCEvaluacionTecnicaContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpGet]
        [Route("Consultar")]
        public async Task<IActionResult> Consultar()
        {
            var responseAPI = new ResponseAPI<List<RegistroPedidosDTO>>();
            try
            {
                var listaPedidos = await _context.Pedidos.Select(x => new RegistroPedidosDTO
                {
                    Pedido= new PedidoDTO
                    {
                        Id=x.Id,
                        FechaHora=x.FechaHora,
                        Cliente=x.Cliente,
                        Vendedor= new UsuarioDTO
                        {
                            Id = x.FkIdVendedorNavigation.Id,
                            Nombre = x.FkIdVendedorNavigation.Nombre,
                            Correo = x.FkIdVendedorNavigation.Correo,
                            Rol = x.FkIdVendedorNavigation.Rol
                        }
                    },
                    Detalles=x.DetallePedidos.Select(y=> new DetallePedidoDTO
                    {
                        FkIdPedido=y.FkIdPedido,
                        FKClaveProducto=y.FkClaveProducto,
                        Cantidad=y.Cantidad,
                        SubTotal=y.SubTotal,
                        Producto= new ProductoDTO
                        {
                            Clave=y.FkIdProductoNavigation.Clave,
                            Nombre = y.FkIdProductoNavigation.Nombre,
                            Existencias = y.FkIdProductoNavigation.Existencias,
                            CostoUnidad = y.FkIdProductoNavigation.CostoUnidad
                        }
                    }).ToList()
                }).ToListAsync();

                responseAPI.EsCorrecto = true;
                responseAPI.Valor = listaPedidos;

                return StatusCode(StatusCodes.Status200OK, responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al cargar los pedidos. Detalles: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }

        [HttpGet]
        [Route("ConsultarVendedor/{vendedor}")]
        public async Task<IActionResult> ConsultarVendedor(int vendedor)
        {
            var responseAPI = new ResponseAPI<List<RegistroPedidosDTO>>();
            try
            {
                //Consulta usando entity framework.
                #region
                /*var listaPedidos = await _context.Pedidos.Where(x=>x.FkIdVendedor==vendedor).Select(x => new RegistroPedidosDTO
                {
                    Pedido = new PedidoDTO
                    {
                        Id = x.Id,
                        FechaHora = x.FechaHora,
                        Cliente = x.Cliente,
                        Vendedor = new UsuarioDTO
                        {
                            Id = x.FkIdVendedorNavigation.Id,
                            Nombre = x.FkIdVendedorNavigation.Nombre,
                            Correo = x.FkIdVendedorNavigation.Correo,
                            Rol = x.FkIdVendedorNavigation.Rol
                        }
                    },
                    Detalles = x.DetallePedidos.Select(y => new DetallePedidoDTO
                    {
                        FkIdPedido = y.FkIdPedido,
                        FKClaveProducto = y.FkClaveProducto,
                        Cantidad = y.Cantidad,
                        SubTotal = y.SubTotal,
                        Producto = new ProductoDTO
                        {
                            Clave = y.FkIdProductoNavigation.Clave,
                            Nombre = y.FkIdProductoNavigation.Nombre,
                            Existencias = y.FkIdProductoNavigation.Existencias,
                            CostoUnidad = y.FkIdProductoNavigation.CostoUnidad
                        }
                    }).ToList()
                }).ToListAsync();*/
                #endregion
                //Consulta haciendo uso de la vista.
                var filas = await _context.RegistroPedidos.Where(r => r.VendedorId == vendedor).ToListAsync();
                var listaPedidos = filas.GroupBy(r => new { r.PedidoId, r.FechaHora, r.Cliente, r.VendedorId, r.VendedorNombre, r.VendedorCorreo, r.VendedorRol }).Select(g => new RegistroPedidosDTO
                {
                    Pedido = new PedidoDTO
                    {
                        Id = g.Key.PedidoId,
                        FechaHora = g.Key.FechaHora,
                        Cliente = g.Key.Cliente,
                        Vendedor = new UsuarioDTO
                        {
                            Id = g.Key.VendedorId,
                            Nombre = g.Key.VendedorNombre,
                            Correo = g.Key.VendedorCorreo,
                            Rol = g.Key.VendedorRol
                        }
                    },
                    Detalles = g.Select(item => new DetallePedidoDTO
                    {
                        FkIdPedido = item.PedidoId,
                        FKClaveProducto = item.ProductoClave,
                        Cantidad = item.Cantidad,
                        SubTotal = item.SubTotal,
                        Producto = new ProductoDTO
                        {
                            Clave = item.ProductoClave,
                            Nombre = item.ProductoNombre,
                            Existencias = item.ProductoExistencias,
                            CostoUnidad = item.ProductoCostoUnidad
                        }
                    }).ToList()
                }).ToList();

                responseAPI.EsCorrecto = true;
                responseAPI.Valor = listaPedidos;

                return StatusCode(StatusCodes.Status200OK, responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.EsCorrecto = false;
                responseAPI.Mensaje = "Ocurrió un error al cargar los pedidos. Detalles: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult<ResponseAPI<List<RegistroPedidosDTO>>>> Guardar(RegistroPedidosDTO pedido)
        {
            var responseAPI = new ResponseAPI<List<RegistroPedidosDTO>>();
            //TRANSACCIÓN.
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //Guardando pedido.
                    var pedidoGuardar = new Pedido
                    {
                        Id=0, //A la espera.
                        FechaHora=pedido.Pedido!.FechaHora,
                        Cliente=pedido.Pedido.Cliente!,
                        FkIdVendedor=pedido.Pedido.FkIdVendedor
                    };
                    await _context.Pedidos.AddAsync(pedidoGuardar);
                    await _context.SaveChangesAsync();

                    //Guardando los detalles.
                    var listaDetallesGuardar = pedido.Detalles!.Select(x=> new DetallePedido
                    {
                        FkIdPedido=pedidoGuardar.Id,
                        FkClaveProducto=x.FKClaveProducto,
                        Cantidad=x.Cantidad,
                        SubTotal=x.SubTotal
                    }).ToList();
                    await _context.DetallePedidos.AddRangeAsync(listaDetallesGuardar);
                    await _context.SaveChangesAsync();

                    //Actualizando las existencias de los productos, esto fue sustituido por el uso de un trigger.
                    #region
                    /*
                    // 1. Preparando un diccionario para acceso rápido: IdProducto → UnidadesSalida.
                    var salidas = listaDetallesGuardar.GroupBy(d => d.FkClaveProducto).ToDictionary
                    (
                        g => g.Key,
                        g => g.Sum(d => d.Cantidad) 
                    );

                    // 2. Obteniendó los productos a actualizar.
                    var productosActualizar = await _context.Productos.Where(p => salidas.Keys.Contains(p.Clave)).ToListAsync();

                    // 3. Recorriendo cada producto y ajustando sus existencias.
                    productosActualizar.ForEach(producto =>
                    {
                        var unidadesSalida = salidas[producto.Clave];
                        producto.Existencias = Math.Max(0, producto.Existencias - unidadesSalida);
                    });
                    await _context.SaveChangesAsync();*/
                    #endregion

                    //Confirmando la transacción.
                    await transaccion.CommitAsync();
                    //Enviamos respuesta.
                    responseAPI.EsCorrecto = true;
                    responseAPI.Valor = await _context.Pedidos.Where(x => x.FkIdVendedor == pedido.Pedido.FkIdVendedor).Select(x => new RegistroPedidosDTO
                    {
                        Pedido = new PedidoDTO
                        {
                            Id = x.Id,
                            FechaHora = x.FechaHora,
                            Cliente = x.Cliente,
                            Vendedor = new UsuarioDTO
                            {
                                Id = x.FkIdVendedorNavigation.Id,
                                Nombre = x.FkIdVendedorNavigation.Nombre,
                                Correo = x.FkIdVendedorNavigation.Correo,
                                Rol = x.FkIdVendedorNavigation.Rol
                            }
                        },
                        Detalles = x.DetallePedidos.Select(y => new DetallePedidoDTO
                        {
                            FkIdPedido = y.FkIdPedido,
                            FKClaveProducto = y.FkClaveProducto,
                            Cantidad = y.Cantidad,
                            SubTotal = y.SubTotal,
                            Producto = new ProductoDTO
                            {
                                Clave = y.FkIdProductoNavigation.Clave,
                                Nombre = y.FkIdProductoNavigation.Nombre,
                                Existencias = y.FkIdProductoNavigation.Existencias,
                                CostoUnidad = y.FkIdProductoNavigation.CostoUnidad
                            }
                        }).ToList()
                    }).ToListAsync();
                    responseAPI.Mensaje = "Pedido guardado exitosamente.";
                    return StatusCode(StatusCodes.Status201Created, responseAPI);
                }
                catch (Exception)
                {
                    //Si fue encontrado un error, se revierte la transacción.
                    await transaccion.RollbackAsync();
                    //Respuesta.
                    responseAPI.EsCorrecto = false;
                    responseAPI.Mensaje = $"Ocurrió un error al guardar el pedido.";
                    return StatusCode(StatusCodes.Status500InternalServerError, responseAPI);
                }
            }
        }
    }
}
