using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data;

/// <summary>
/// Contexto de base de datos de la veterinaria El Arca de Moe.
/// Compatible con SQL Server, SQLite y cualquier proveedor EF Core.
/// El proveedor concreto se configura desde Program.cs según appsettings.json.
/// </summary>
public class VeterinariaDbContext : DbContext
{
    public VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options)
        : base(options) { }

    // ── DbSets (tablas) ──────────────────────────────────────────────
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Mascota>     Mascotas     { get; set; }
    public DbSet<Veterinario> Veterinarios { get; set; }
    public DbSet<Cita>        Citas        { get; set; }

    // ── Configuración del modelo ─────────────────────────────────────
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Propietario ──────────────────────────────────────────────
        modelBuilder.Entity<Propietario>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre)   .IsRequired().HasMaxLength(50);
            e.Property(p => p.Apellido) .IsRequired().HasMaxLength(50);
            e.Property(p => p.Telefono) .IsRequired().HasMaxLength(15);
            e.Property(p => p.Correo)   .IsRequired().HasMaxLength(100);
            e.Property(p => p.Estado)   .HasConversion<int>();

            // NombreCompleto es calculado en C#, no se persiste en BD
            e.Ignore(p => p.NombreCompleto);
        });

        // ── Veterinario ──────────────────────────────────────────────
        modelBuilder.Entity<Veterinario>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Nombre)       .IsRequired().HasMaxLength(50);
            e.Property(v => v.Apellido)     .IsRequired().HasMaxLength(50);
            e.Property(v => v.Especialidad) .IsRequired().HasMaxLength(80);
            e.Property(v => v.Telefono)     .IsRequired().HasMaxLength(15);
            e.Property(v => v.Estado)       .HasConversion<int>();

            e.Ignore(v => v.NombreCompleto);
        });

        // ── Mascota ──────────────────────────────────────────────────
        modelBuilder.Entity<Mascota>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Nombre)  .IsRequired().HasMaxLength(50);
            e.Property(m => m.Especie) .IsRequired().HasMaxLength(50);
            e.Property(m => m.Raza)    .HasMaxLength(50);
            e.Property(m => m.Color)   .HasMaxLength(30);
            e.Property(m => m.Estado)  .HasConversion<int>();

            // Relación Mascota → Propietario (muchas mascotas, un propietario)
            e.HasOne(m => m.Propietario)
             .WithMany()
             .HasForeignKey(m => m.PropietarioId)
             .OnDelete(DeleteBehavior.Restrict);  // no eliminar en cascada

            e.Ignore(m => m.Edad);
        });

        // ── Cita ─────────────────────────────────────────────────────
        modelBuilder.Entity<Cita>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Motivo)     .IsRequired().HasMaxLength(200);
            e.Property(c => c.Diagnostico).HasMaxLength(500);
            e.Property(c => c.Estado)     .HasConversion<int>();

            // Relación Cita → Mascota
            e.HasOne(c => c.Mascota)
             .WithMany()
             .HasForeignKey(c => c.MascotaId)
             .OnDelete(DeleteBehavior.Restrict);

            // Relación Cita → Veterinario
            e.HasOne(c => c.Veterinario)
             .WithMany()
             .HasForeignKey(c => c.VeterinarioId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Ignore(c => c.FechaHoraFormateada);
        });
    }
}
