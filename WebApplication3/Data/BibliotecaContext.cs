using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario - Prestamo
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Libro - Prestamo
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Datos semilla - Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Correo = "admin@upv.edu.bo",
                    Contrasena = "admin123",
                    Rol = "Administrador",
                    Activo = true
                },
                new Usuario
                {
                    Id = 2,
                    Nombre = "María Flores",
                    Correo = "bibliotecario@upv.edu.bo",
                    Contrasena = "biblio123",
                    Rol = "Bibliotecario",
                    Activo = true
                },
                new Usuario
                {
                    Id = 3,
                    Nombre = "Juan Pérez",
                    Correo = "usuario@upv.edu.bo",
                    Contrasena = "usuario123",
                    Rol = "Usuario",
                    Activo = true
                }
            );

            // Datos semilla - Libros
            modelBuilder.Entity<Libro>().HasData(
                new Libro
                {
                    Id = 1,
                    Titulo = "Introducción a la Programación",
                    Autor = "Carlos García",
                    ISBN = "978-9999-01-001-1",
                    Categoria = "Tecnología",
                    CantidadDisponible = 5,
                    AnioPublicacion = 2020
                },
                new Libro
                {
                    Id = 2,
                    Titulo = "Derecho Civil Boliviano",
                    Autor = "Roberto Méndez",
                    ISBN = "978-9999-01-002-2",
                    Categoria = "Derecho",
                    CantidadDisponible = 3,
                    AnioPublicacion = 2018
                },
                new Libro
                {
                    Id = 3,
                    Titulo = "Administración de Empresas",
                    Autor = "Ana Rodríguez",
                    ISBN = "978-9999-01-003-3",
                    Categoria = "Administración",
                    CantidadDisponible = 4,
                    AnioPublicacion = 2021
                },
                new Libro
                {
                    Id = 4,
                    Titulo = "Matemáticas Discretas",
                    Autor = "Luis Vargas",
                    ISBN = "978-9999-01-004-4",
                    Categoria = "Matemáticas",
                    CantidadDisponible = 2,
                    AnioPublicacion = 2019
                }
            );
        }
    }
}
