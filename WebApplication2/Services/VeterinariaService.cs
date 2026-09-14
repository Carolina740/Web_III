using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Services;
public class VeterinariaService
{
    private readonly VeterinariaDbContext _db;

    public VeterinariaService(VeterinariaDbContext db) => _db = db;

    public List<Propietario> ObtenerPropietarios() =>
        _db.Propietarios
           .OrderBy(p => p.Apellido).ThenBy(p => p.Nombre)
           .ToList();

    public Propietario? ObtenerPropietarioPorId(int id) =>
        _db.Propietarios.FirstOrDefault(p => p.Id == id);

    public void AgregarPropietario(Propietario propietario)
    {
        _db.Propietarios.Add(propietario);
        _db.SaveChanges();
    }

    public bool ActualizarPropietario(Propietario propietario)
    {
        var existente = _db.Propietarios.Find(propietario.Id);
        if (existente is null) return false;

        existente.Nombre   = propietario.Nombre;
        existente.Apellido = propietario.Apellido;
        existente.Telefono = propietario.Telefono;
        existente.Correo   = propietario.Correo;
        existente.Estado   = propietario.Estado;

        _db.SaveChanges();
        return true;
    }

    public bool EliminarPropietario(int id)
    {
        var existente = _db.Propietarios.Find(id);
        if (existente is null) return false;

        _db.Propietarios.Remove(existente);
        _db.SaveChanges();
        return true;
    }

    public bool TieneMascotas(int propietarioId) =>
        _db.Mascotas.Any(m => m.PropietarioId == propietarioId);

    public List<Mascota> ObtenerMascotas(bool resolverNavegacion = true)
    {
        var query = _db.Mascotas.OrderBy(m => m.Nombre);

        if (resolverNavegacion)
            return query.Include(m => m.Propietario).ToList();

        return query.ToList();
    }

    public List<Mascota> ObtenerMascotasPorPropietario(int propietarioId) =>
        _db.Mascotas
           .Where(m => m.PropietarioId == propietarioId)
           .Include(m => m.Propietario)
           .OrderBy(m => m.Nombre)
           .ToList();

    public Mascota? ObtenerMascotaPorId(int id, bool resolverNavegacion = true)
    {
        if (resolverNavegacion)
            return _db.Mascotas
                      .Include(m => m.Propietario)
                      .FirstOrDefault(m => m.Id == id);

        return _db.Mascotas.Find(id);
    }

    public void AgregarMascota(Mascota mascota)
    {
        _db.Mascotas.Add(mascota);
        _db.SaveChanges();
    }

    public bool ActualizarMascota(Mascota mascota)
    {
        var existente = _db.Mascotas.Find(mascota.Id);
        if (existente is null) return false;

        existente.Nombre          = mascota.Nombre;
        existente.PropietarioId   = mascota.PropietarioId;
        existente.Especie         = mascota.Especie;
        existente.Raza            = mascota.Raza;
        existente.FechaNacimiento = mascota.FechaNacimiento;
        existente.Color           = mascota.Color;
        existente.Estado          = mascota.Estado;

        _db.SaveChanges();
        return true;
    }

    public bool EliminarMascota(int id)
    {
        var existente = _db.Mascotas.Find(id);
        if (existente is null) return false;

        _db.Mascotas.Remove(existente);
        _db.SaveChanges();
        return true;
    }

    public bool TieneCitas(int mascotaId) =>
        _db.Citas.Any(c => c.MascotaId == mascotaId);

    public List<Veterinario> ObtenerVeterinarios() =>
        _db.Veterinarios
           .OrderBy(v => v.Apellido).ThenBy(v => v.Nombre)
           .ToList();

    public Veterinario? ObtenerVeterinarioPorId(int id) =>
        _db.Veterinarios.Find(id);

    public void AgregarVeterinario(Veterinario veterinario)
    {
        _db.Veterinarios.Add(veterinario);
        _db.SaveChanges();
    }

    public bool ActualizarVeterinario(Veterinario veterinario)
    {
        var existente = _db.Veterinarios.Find(veterinario.Id);
        if (existente is null) return false;

        existente.Nombre       = veterinario.Nombre;
        existente.Apellido     = veterinario.Apellido;
        existente.Especialidad = veterinario.Especialidad;
        existente.Telefono     = veterinario.Telefono;
        existente.Estado       = veterinario.Estado;

        _db.SaveChanges();
        return true;
    }

    public bool EliminarVeterinario(int id)
    {
        var existente = _db.Veterinarios.Find(id);
        if (existente is null) return false;

        _db.Veterinarios.Remove(existente);
        _db.SaveChanges();
        return true;
    }

    public List<Cita> ObtenerCitas(bool resolverNavegacion = true)
    {
        if (resolverNavegacion)
            return _db.Citas
                      .Include(c => c.Mascota).ThenInclude(m => m!.Propietario)
                      .Include(c => c.Veterinario)
                      .OrderByDescending(c => c.FechaHora)
                      .ToList();

        return _db.Citas.OrderByDescending(c => c.FechaHora).ToList();
    }

    public List<Cita> ObtenerCitasPendientes() =>
        _db.Citas
           .Where(c => c.Estado == EstadoCita.Pendiente && c.FechaHora >= DateTime.Today)
           .Include(c => c.Mascota).ThenInclude(m => m!.Propietario)
           .Include(c => c.Veterinario)
           .OrderBy(c => c.FechaHora)
           .ToList();

    public Cita? ObtenerCitaPorId(int id, bool resolverNavegacion = true)
    {
        if (resolverNavegacion)
            return _db.Citas
                      .Include(c => c.Mascota).ThenInclude(m => m!.Propietario)
                      .Include(c => c.Veterinario)
                      .FirstOrDefault(c => c.Id == id);

        return _db.Citas.Find(id);
    }

    public void AgregarCita(Cita cita)
    {
        _db.Citas.Add(cita);
        _db.SaveChanges();
    }

    public bool ActualizarCita(Cita cita)
    {
        var existente = _db.Citas.Find(cita.Id);
        if (existente is null) return false;

        existente.MascotaId    = cita.MascotaId;
        existente.VeterinarioId = cita.VeterinarioId;
        existente.FechaHora    = cita.FechaHora;
        existente.Motivo       = cita.Motivo;
        existente.Estado       = cita.Estado;
        existente.Diagnostico  = cita.Diagnostico;

        _db.SaveChanges();
        return true;
    }

    public bool EliminarCita(int id)
    {
        var existente = _db.Citas.Find(id);
        if (existente is null) return false;

        _db.Citas.Remove(existente);
        _db.SaveChanges();
        return true;
    }

    public DashboardStats ObtenerEstadisticas() => new DashboardStats
    {
        TotalPropietarios   = _db.Propietarios.Count(),
        PropietariosActivos = _db.Propietarios.Count(p => p.Estado == EstadoGeneral.Activo),
        TotalMascotas       = _db.Mascotas.Count(),
        MascotasActivas     = _db.Mascotas.Count(m => m.Estado == EstadoGeneral.Activo),
        TotalVeterinarios   = _db.Veterinarios.Count(),
        VeterinariosActivos = _db.Veterinarios.Count(v => v.Estado == EstadoGeneral.Activo),
        CitasPendientes     = _db.Citas.Count(c => c.Estado == EstadoCita.Pendiente),
        CitasCompletadas    = _db.Citas.Count(c => c.Estado == EstadoCita.Completada),
        CitasCanceladas     = _db.Citas.Count(c => c.Estado == EstadoCita.Cancelada),
        ProximasCitas       = ObtenerCitasPendientes().Take(5).ToList()
    };
}
public class DashboardStats
{
    public int TotalPropietarios   { get; init; }
    public int PropietariosActivos { get; init; }
    public int TotalMascotas       { get; init; }
    public int MascotasActivas     { get; init; }
    public int TotalVeterinarios   { get; init; }
    public int VeterinariosActivos { get; init; }
    public int CitasPendientes     { get; init; }
    public int CitasCompletadas    { get; init; }
    public int CitasCanceladas     { get; init; }
    public List<Cita> ProximasCitas { get; init; } = new();
}
