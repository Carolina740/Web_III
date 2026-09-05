using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data;

public class VeterinariaDbContext : DbContext
{
    public VeterinariaDbContext(DbContextOptions<VeterinariaDbContext> options)
        : base(options) { }

    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Mascota>     Mascotas     { get; set; }
    public DbSet<Veterinario> Veterinarios { get; set; }
    public DbSet<Cita>        Citas        { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Propietario>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre)   .IsRequired().HasMaxLength(50);
            e.Property(p => p.Apellido) .IsRequired().HasMaxLength(50);
            e.Property(p => p.Telefono) .IsRequired().HasMaxLength(15);
            e.Property(p => p.Correo)   .IsRequired().HasMaxLength(100);
            e.Property(p => p.Estado)   .HasConversion<int>();
            e.Ignore(p => p.NombreCompleto);
        });

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

        modelBuilder.Entity<Mascota>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Nombre)  .IsRequired().HasMaxLength(50);
            e.Property(m => m.Especie) .IsRequired().HasMaxLength(50);
            e.Property(m => m.Raza)    .HasMaxLength(50);
            e.Property(m => m.Color)   .HasMaxLength(30);
            e.Property(m => m.Estado)  .HasConversion<int>();
            e.HasOne(m => m.Propietario)
             .WithMany()
             .HasForeignKey(m => m.PropietarioId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Ignore(m => m.Edad);
        });

        modelBuilder.Entity<Cita>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Motivo)     .IsRequired().HasMaxLength(200);
            e.Property(c => c.Diagnostico).HasMaxLength(500);
            e.Property(c => c.Estado)     .HasConversion<int>();
            e.HasOne(c => c.Mascota)
             .WithMany()
             .HasForeignKey(c => c.MascotaId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Veterinario)
             .WithMany()
             .HasForeignKey(c => c.VeterinarioId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Ignore(c => c.FechaHoraFormateada);
        });
    }
}
