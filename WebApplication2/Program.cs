using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using WebApplication2.Data;
using WebApplication2.Middleware;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Razor Pages con Antiforgery global ───────────────────────────────────────
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Propietarios");
    options.Conventions.AuthorizeFolder("/Mascotas");
    options.Conventions.AuthorizeFolder("/Veterinarios");
    options.Conventions.AuthorizeFolder("/Citas");
    options.Conventions.AuthorizePage("/Index");

    options.Conventions.AllowAnonymousToFolder("/Account");
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Error");
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly     = true;
    options.Cookie.SameSite     = SameSiteMode.Strict;
});

// ── Entity Framework Core ─────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró 'DefaultConnection' en appsettings.json.");

var dbProvider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";

builder.Services.AddDbContext<VeterinariaDbContext>(options =>
{
    _ = dbProvider switch
    {
        "SqlServer" => options.UseSqlServer(connectionString),
        "Sqlite"    => options.UseSqlite(connectionString),
        _ => throw new InvalidOperationException(
            $"Proveedor de BD '{dbProvider}' no reconocido. Valores: SqlServer, Sqlite.")
    };
});

// ── ASP.NET Core Identity ─────────────────────────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Contraseña
    options.Password.RequireDigit           = true;
    options.Password.RequireLowercase       = true;
    options.Password.RequireUppercase       = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength         = 6;

    // Bloqueo tras intentos fallidos
    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers      = true;

    // Usuario
    options.User.RequireUniqueEmail = true;

    // No requerir confirmación de email (se puede activar después)
    options.SignIn.RequireConfirmedEmail   = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<VeterinariaDbContext>()
.AddDefaultTokenProviders();

// Cookie de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath           = "/Account/Login";
    options.LogoutPath          = "/Account/Logout";
    options.AccessDeniedPath    = "/Account/AccesoDenegado";
    options.ExpireTimeSpan      = TimeSpan.FromHours(8);
    options.SlidingExpiration   = true;
    options.Cookie.HttpOnly     = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite     = SameSiteMode.Strict;
    options.Cookie.Name         = ".ArcaDeMoe.Auth";
});

// ── Servicio de datos ─────────────────────────────────────────────────────────
builder.Services.AddScoped<VeterinariaService>();

// ── Rate Limiting — protege contra fuerza bruta y abuso ──────────────────────
builder.Services.AddRateLimiter(rl =>
{
    // Límite general: 100 req/min por IP
    rl.AddFixedWindowLimiter("general", opt =>
    {
        opt.PermitLimit         = 100;
        opt.Window              = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit          = 10;
    });

    // Límite estricto para login: 10 intentos/min por IP
    rl.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit         = 10;
        opt.Window              = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit          = 2;
    });

    rl.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();
// ─────────────────────────────────────────────────────────────────────────────

// ── Migraciones y seed al arrancar ───────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db          = scope.ServiceProvider.GetRequiredService<VeterinariaDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    db.Database.Migrate();
    await SeedDatos(db);
    await SeedIdentity(userManager, roleManager);
}

// ── Pipeline de seguridad (orden importa) ────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSecurityHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets().RequireRateLimiting("general");

app.Run();

// =============================================================================
// SEED: datos de ejemplo de la clínica
// =============================================================================
static async Task SeedDatos(VeterinariaDbContext db)
{
    if (db.Propietarios.Any()) return;

    var p1 = new Propietario { Nombre = "María",  Apellido = "González",  Telefono = "3001234567", Correo = "maria.gonzalez@email.com",  Estado = EstadoGeneral.Activo   };
    var p2 = new Propietario { Nombre = "Carlos", Apellido = "Rodríguez", Telefono = "3109876543", Correo = "carlos.rodriguez@email.com", Estado = EstadoGeneral.Activo   };
    var p3 = new Propietario { Nombre = "Laura",  Apellido = "Martínez",  Telefono = "3205551234", Correo = "laura.martinez@email.com",   Estado = EstadoGeneral.Inactivo };
    db.Propietarios.AddRange(p1, p2, p3);
    await db.SaveChangesAsync();

    var v1 = new Veterinario { Nombre = "Andrés", Apellido = "Pereira", Especialidad = "Medicina General",    Telefono = "3001112222", Estado = EstadoGeneral.Activo };
    var v2 = new Veterinario { Nombre = "Sofía",  Apellido = "Ramírez", Especialidad = "Cirugía Veterinaria", Telefono = "3003334444", Estado = EstadoGeneral.Activo };
    var v3 = new Veterinario { Nombre = "Felipe", Apellido = "Castro",  Especialidad = "Dermatología Animal", Telefono = "3005556666", Estado = EstadoGeneral.Activo };
    db.Veterinarios.AddRange(v1, v2, v3);
    await db.SaveChangesAsync();

    var m1 = new Mascota { Nombre = "Max",   PropietarioId = p1.Id, Especie = "Perro", Raza = "Labrador",        FechaNacimiento = new DateTime(2020, 3, 15),  Color = "Amarillo", Estado = EstadoGeneral.Activo   };
    var m2 = new Mascota { Nombre = "Luna",  PropietarioId = p1.Id, Especie = "Gato",  Raza = "Siamés",          FechaNacimiento = new DateTime(2021, 7, 20),  Color = "Blanco",   Estado = EstadoGeneral.Activo   };
    var m3 = new Mascota { Nombre = "Rocky", PropietarioId = p2.Id, Especie = "Perro", Raza = "Bulldog Francés", FechaNacimiento = new DateTime(2019, 11, 5),  Color = "Atigrado", Estado = EstadoGeneral.Activo   };
    var m4 = new Mascota { Nombre = "Coco",  PropietarioId = p3.Id, Especie = "Ave",   Raza = "Canario",         FechaNacimiento = new DateTime(2022, 1, 10),  Color = "Amarillo", Estado = EstadoGeneral.Inactivo };
    db.Mascotas.AddRange(m1, m2, m3, m4);
    await db.SaveChangesAsync();

    var c1 = new Cita { MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(-5), Motivo = "Vacunación anual",       Estado = EstadoCita.Completada, Diagnostico = "Paciente en buen estado. Vacunas al día."     };
    var c2 = new Cita { MascotaId = m2.Id, VeterinarioId = v2.Id, FechaHora = DateTime.Now.AddDays(2),  Motivo = "Revisión postoperatoria", Estado = EstadoCita.Pendiente,  Diagnostico = null                                           };
    var c3 = new Cita { MascotaId = m3.Id, VeterinarioId = v3.Id, FechaHora = DateTime.Now.AddDays(-1), Motivo = "Problema de piel",        Estado = EstadoCita.Completada, Diagnostico = "Dermatitis leve. Se receta shampoo medicado." };
    var c4 = new Cita { MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(7),  Motivo = "Control de peso",         Estado = EstadoCita.Pendiente,  Diagnostico = null                                           };
    db.Citas.AddRange(c1, c2, c3, c4);
    await db.SaveChangesAsync();
}

// =============================================================================
// SEED: usuario administrador por defecto
// =============================================================================
static async Task SeedIdentity(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole>    roleManager)
{
    const string rolAdmin   = "Admin";
    const string emailAdmin = "admin@arcamoe.com";
    const string passAdmin  = "Admin123!";

    if (!await roleManager.RoleExistsAsync(rolAdmin))
        await roleManager.CreateAsync(new IdentityRole(rolAdmin));

    if (await userManager.FindByEmailAsync(emailAdmin) is null)
    {
        var admin = new ApplicationUser
        {
            UserName = emailAdmin,
            Email    = emailAdmin,
            Nombre   = "Administrador",
            Apellido = "Sistema",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, passAdmin);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, rolAdmin);
    }
}
