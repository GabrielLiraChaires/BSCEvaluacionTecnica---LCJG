using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[PrimaryKey("FkIdPedido", "FkIdProducto")]
[Table("DetallePedido")]
public partial class DetallePedido
{
    [Key]
    public int FkIdPedido { get; set; }

    [Key]
    public int FkIdProducto { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal { get; set; }

    [ForeignKey("FkIdPedido")]
    [InverseProperty("DetallePedidos")]
    public virtual Pedido FkIdPedidoNavigation { get; set; } = null!;

    [ForeignKey("FkIdProducto")]
    [InverseProperty("DetallePedidos")]
    public virtual Producto FkIdProductoNavigation { get; set; } = null!;
}
