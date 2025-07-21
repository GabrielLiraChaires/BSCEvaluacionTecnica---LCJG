using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities
{
    [PrimaryKey("FkIdPedido", "FkClaveProducto")]
    [Table("DetallePedido")]
    public partial class DetallePedido
    {
        [Key]
        public int FkIdPedido { get; set; }

        [Key]
        public string FkClaveProducto { get; set; } = null!;

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotal { get; set; }

        [ForeignKey(nameof(FkIdPedido))]
        [InverseProperty("DetallePedidos")]
        public virtual Pedido FkIdPedidoNavigation { get; set; } = null!;

        // ← Aquí corregimos el nombre de la FK
        [ForeignKey(nameof(FkClaveProducto))]
        [InverseProperty("DetallePedidos")]
        public virtual Producto FkIdProductoNavigation { get; set; } = null!;
    }
}
