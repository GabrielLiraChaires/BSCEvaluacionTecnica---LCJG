using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class RegistroPedidoDTO
    {
        public int PedidoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Cliente { get; set; } = null!;
        public int VendedorId { get; set; }
        public string VendedorNombre { get; set; } = null!;
        public string VendedorCorreo { get; set; } = null!;
        public string VendedorRol { get; set; } = null!;
        public string ProductoClave { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public int ProductoExistencias { get; set; }
        public decimal ProductoCostoUnidad { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }

}
