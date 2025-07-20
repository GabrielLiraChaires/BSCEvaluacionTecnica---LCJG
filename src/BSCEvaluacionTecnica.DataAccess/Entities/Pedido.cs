using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[Table("Pedido")]
public partial class Pedido
{
    [Key]
    public int Id { get; set; }

    [Precision(0)]
    public DateTime FechaHora { get; set; }

    [StringLength(100)]
    public string Cliente { get; set; } = null!;

    public int FkIdVendedor { get; set; }

    [InverseProperty("FkIdPedidoNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    [ForeignKey("FkIdVendedor")]
    [InverseProperty("Pedidos")]
    public virtual Usuario FkIdVendedorNavigation { get; set; } = null!;
}
