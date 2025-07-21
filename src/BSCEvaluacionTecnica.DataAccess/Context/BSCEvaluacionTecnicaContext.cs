using System;
using System.Collections.Generic;
using BSCEvaluacionTecnica.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using BSCEvaluacionTecnica.Shared.DTOs;

namespace BSCEvaluacionTecnica.DataAccess.Context;

public partial class BSCEvaluacionTecnicaContext : DbContext
{
    public BSCEvaluacionTecnicaContext()
    {
    }

    public BSCEvaluacionTecnicaContext(DbContextOptions<BSCEvaluacionTecnicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<ModuloSistema> ModuloSistemas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PermisoModuloUsuario> PermisoModuloUsuarios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    
    public DbSet<RegistroPedidoDTO> RegistroPedidos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => new { e.FkIdPedido, e.FkClaveProducto });

            entity.HasOne(d => d.FkIdPedidoNavigation)
                  .WithMany(p => p.DetallePedidos)
                  .HasForeignKey(d => d.FkIdPedido)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FKDetallePedido_Pedido");

            entity.HasOne(d => d.FkIdProductoNavigation)
                  .WithMany(p => p.DetallePedidos)
                  .HasForeignKey(d => d.FkClaveProducto)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FKDetallePedido_Producto");
        });

        modelBuilder.Entity<ModuloSistema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ModuloSi__3214EC07A48BA183");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pedido__3214EC0725BCC797");

            entity.Property(e => e.FechaHora).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.FkIdVendedorNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKIdVendedor");
        });

        modelBuilder.Entity<PermisoModuloUsuario>(entity =>
        {
            entity.HasKey(e => new { e.FkIdUsuario, e.FkIdModuloSistema }).HasName("PK__PermisoM__9F59518F74DA1D5B");

            entity.HasOne(d => d.FkIdModuloSistemaNavigation).WithMany(p => p.PermisoModuloUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdModuloSistema");

            entity.HasOne(d => d.FkIdUsuarioNavigation).WithMany(p => p.PermisoModuloUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdUsuario");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Clave).HasName("PK__Producto__3214EC0754402FD2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07F810EF89");
        });

        //Vista.
        modelBuilder.Entity<RegistroPedidoDTO>(eb =>
        {
            eb.HasNoKey();
            eb.ToView("vw_RegistroPedidos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
