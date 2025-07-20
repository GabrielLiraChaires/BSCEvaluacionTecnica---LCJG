using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[Table("Usuario")]
[Index("Nombre", Name = "UQ__Usuario__75E3EFCF48E82DF1", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string Correo { get; set; } = null!;

    [StringLength(256)]
    public string Contrasena { get; set; } = null!;

    [StringLength(23)]
    public string Rol { get; set; } = null!;

    [InverseProperty("FkIdVendedorNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [InverseProperty("FkIdUsuarioNavigation")]
    public virtual ICollection<PermisoModuloUsuario> PermisoModuloUsuarios { get; set; } = new List<PermisoModuloUsuario>();
}
