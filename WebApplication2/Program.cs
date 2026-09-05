using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró 'DefaultConnection' en appsettings.json. " +
        "Por favor configura la cadena de conexión antes de ejecutar la app.");

var dbProvider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";

builder.Services.AddDbContext<VeterinariaDbContext>(options =>
{
    _ = dbProvider switch
    {
        "SqlServer"  => options.UseSqlServer(connectionString),
        "Sqlite"     => options.UseSqlite(connectionString),
        _ => throw new InvalidOperationException(
            $"Proveedor de BD '{dbProvider}' no reconocido. " +
            "Valores válidos: SqlServer, Sqlite, MySql, PostgreSQL")
    };
});
builder.Services.AddScoped<VeterinariaService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VeterinariaDbContext>();
    db.Database.Migrate();
    SeedDatos(db);
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
static void SeedDatos(VeterinariaDbContext db)
{
    if (db.Propietarios.Any()) return;
    var p1 = new Propietario { Nombre = "María",  Apellido = "González",  Telefono = "3001234567", Correo = "maria.gonzalez@email.com",  Estado = EstadoGeneral.Activo   };
    var p2 = new Propietario { Nombre = "Carlos", Apellido = "Rodríguez", Telefono = "3109876543", Correo = "carlos.rodriguez@email.com", Estado = EstadoGeneral.Activo   };
    var p3 = new Propietario { Nombre = "Laura",  Apellido = "Martínez",  Telefono = "3205551234", Correo = "laura.martinez@email.com",   Estado = EstadoGeneral.Inactivo };
    db.Propietarios.AddRange(p1, p2, p3);
    db.SaveChanges();
    var v1 = new Veterinario { Nombre = "Andrés", Apellido = "Pereira", Especialidad = "Medicina General",    Telefono = "3001112222", Estado = EstadoGeneral.Activo };
    var v2 = new Veterinario { Nombre = "Sofía",  Apellido = "Ramírez", Especialidad = "Cirugía Veterinaria", Telefono = "3003334444", Estado = EstadoGeneral.Activo };
    var v3 = new Veterinario { Nombre = "Felipe", Apellido = "Castro",  Especialidad = "Dermatología Animal", Telefono = "3005556666", Estado = EstadoGeneral.Activo };
    db.Veterinarios.AddRange(v1, v2, v3);
    db.SaveChanges();
    var m1 = new Mascota { Nombre = "Max",   PropietarioId = p1.Id, Especie = "Perro", Raza = "Labrador",        FechaNacimiento = new DateTime(2020, 3, 15),  Color = "Amarillo", Estado = EstadoGeneral.Activo   };
    var m2 = new Mascota { Nombre = "Luna",  PropietarioId = p1.Id, Especie = "Gato",  Raza = "Siamés",          FechaNacimiento = new DateTime(2021, 7, 20),  Color = "Blanco",   Estado = EstadoGeneral.Activo   };
    var m3 = new Mascota { Nombre = "Rocky", PropietarioId = p2.Id, Especie = "Perro", Raza = "Bulldog Francés", FechaNacimiento = new DateTime(2019, 11, 5),  Color = "Atigrado", Estado = EstadoGeneral.Activo   };
    var m4 = new Mascota { Nombre = "Coco",  PropietarioId = p3.Id, Especie = "Ave",   Raza = "Canario",         FechaNacimiento = new DateTime(2022, 1, 10),  Color = "Amarillo", Estado = EstadoGeneral.Inactivo };
    db.Mascotas.AddRange(m1, m2, m3, m4);
    db.SaveChanges();
    var c1 = new Cita { MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(-5), Motivo = "Vacunación anual",       Estado = EstadoCita.Completada, Diagnostico = "Paciente en buen estado. Vacunas al día."         };
    var c2 = new Cita { MascotaId = m2.Id, VeterinarioId = v2.Id, FechaHora = DateTime.Now.AddDays(2),  Motivo = "Revisión postoperatoria", Estado = EstadoCita.Pendiente,  Diagnostico = null                                               };
    var c3 = new Cita { MascotaId = m3.Id, VeterinarioId = v3.Id, FechaHora = DateTime.Now.AddDays(-1), Motivo = "Problema de piel",        Estado = EstadoCita.Completada, Diagnostico = "Dermatitis leve. Se receta shampoo medicado."     };
    var c4 = new Cita { MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(7),  Motivo = "Control de peso",         Estado = EstadoCita.Pendiente,  Diagnostico = null                                               };
    db.Citas.AddRange(c1, c2, c3, c4);
    db.SaveChanges();
}
