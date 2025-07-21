using System;
using System.Collections.Generic;
using System.Linq;
using BSCEvaluacionTecnica.Shared.DTOs;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IPedidosService
    {
        Task<List<RegistroPedidosDTO>> HistorialPedidos();
        Task<List<RegistroPedidosDTO>> HistorialPedidosVendedor(int vendedor);
        Task<ResponseAPI<List<RegistroPedidosDTO>>> Guardar(RegistroPedidosDTO pedido);
    }
}
