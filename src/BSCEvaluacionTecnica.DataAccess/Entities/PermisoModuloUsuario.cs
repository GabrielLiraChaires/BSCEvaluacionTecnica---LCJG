using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BSCEvaluacionTecnica.DataAccess.Entities;

[PrimaryKey("FkIdUsuario", "FkIdModuloSistema")]
[Table("PermisoModuloUsuario")]
public partial class PermisoModuloUsuario
{
    [Key]
    public int FkIdUsuario { get; set; }

    [Key]
    public int FkIdModuloSistema { get; set; }

    [StringLength(9)]
    public string Acceso { get; set; } = null!;

    [ForeignKey("FkIdModuloSistema")]
    [InverseProperty("PermisoModuloUsuarios")]
    public virtual ModuloSistema FkIdModuloSistemaNavigation { get; set; } = null!;

    [ForeignKey("FkIdUsuario")]
    [InverseProperty("PermisoModuloUsuarios")]
    public virtual Usuario FkIdUsuarioNavigation { get; set; } = null!;
}
