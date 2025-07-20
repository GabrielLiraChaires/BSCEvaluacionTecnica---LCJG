using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[Table("ModuloSistema")]
[Index("Nombre", Name = "UQ__ModuloSi__75E3EFCF3AB5C308", IsUnique = true)]
public partial class ModuloSistema
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    [InverseProperty("FkIdModuloSistemaNavigation")]
    public virtual ICollection<PermisoModuloUsuario> PermisoModuloUsuarios { get; set; } = new List<PermisoModuloUsuario>();
}
