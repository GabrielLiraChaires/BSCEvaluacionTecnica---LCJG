using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class DetallePedidoDTO
    {
        public int FkIdPedido { get; set; }
        public string? FKClaveProducto { get; set; }
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotal { get; set; }
        public ProductoDTO? Producto { get; set; }
    }
}
