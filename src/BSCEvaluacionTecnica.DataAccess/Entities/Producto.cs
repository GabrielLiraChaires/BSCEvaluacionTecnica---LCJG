using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[Table("Producto")]
public partial class Producto
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    public int Existencias { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoUnidad { get; set; }

    [InverseProperty("FkIdProductoNavigation")]
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
