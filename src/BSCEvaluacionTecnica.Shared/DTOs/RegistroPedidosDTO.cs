using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class RegistroPedidosDTO
    {
        public PedidoDTO? Pedido { get; set; }
        public List<DetallePedidoDTO>? Detalles { get; set; }
    }
}
