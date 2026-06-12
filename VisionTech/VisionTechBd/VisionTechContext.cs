using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VisionTech.Models;

namespace VisionTech.VisionTechBd;

public partial class VisionTechContext : DbContext
{
    public VisionTechContext()
    {
    }

    public VisionTechContext(DbContextOptions<VisionTechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VisionTechBd;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A10E2B6CB37");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdProduto).HasName("PK__Produto__2E883C23F9408282");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Produtos).HasConstraintName("FK__Produto__IdCateg__4CA06362");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97E4AEC2C6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
