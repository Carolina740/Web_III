using WebApplication2.Models;

namespace WebApplication2.Services;

/// <summary>
/// Servicio singleton que almacena todos los datos en memoria.
/// Simula una capa de repositorio sin necesidad de base de datos.
/// </summary>
public class VeterinariaService
{
    // -----------------------------------------------------------------------
    // Contadores de IDs auto-incremental
    // -----------------------------------------------------------------------
    private int _nextPropietarioId = 1;
    private int _nextMascotaId    = 1;
    private int _nextVeterinarioId = 1;
    private int _nextCitaId       = 1;

    // -----------------------------------------------------------------------
    // Almacenes en memoria
    // -----------------------------------------------------------------------
    private readonly List<Propietario> _propietarios  = new();
    private readonly List<Mascota>     _mascotas      = new();
    private readonly List<Veterinario> _veterinarios  = new();
    private readonly List<Cita>        _citas         = new();

    // -----------------------------------------------------------------------
    // Constructor: datos de ejemplo para arrancar con contenido visible
    // -----------------------------------------------------------------------
    public VeterinariaService()
    {
        SeedDatos();
    }

    private void SeedDatos()
    {
        // --- Propietarios ---
        var p1 = new Propietario { Id = _nextPropietarioId++, Nombre = "María", Apellido = "González",  Telefono = "3001234567", Correo = "maria.gonzalez@email.com",  Estado = EstadoGeneral.Activo   };
        var p2 = new Propietario { Id = _nextPropietarioId++, Nombre = "Carlos",Apellido = "Rodríguez", Telefono = "3109876543", Correo = "carlos.rodriguez@email.com", Estado = EstadoGeneral.Activo   };
        var p3 = new Propietario { Id = _nextPropietarioId++, Nombre = "Laura", Apellido = "Martínez",  Telefono = "3205551234", Correo = "laura.martinez@email.com",   Estado = EstadoGeneral.Inactivo };
        _propietarios.AddRange(new[] { p1, p2, p3 });

        // --- Veterinarios ---
        var v1 = new Veterinario { Id = _nextVeterinarioId++, Nombre = "Andrés",  Apellido = "Pereira",   Especialidad = "Medicina General",    Telefono = "3001112222", Estado = EstadoGeneral.Activo };
        var v2 = new Veterinario { Id = _nextVeterinarioId++, Nombre = "Sofía",   Apellido = "Ramírez",   Especialidad = "Cirugía Veterinaria", Telefono = "3003334444", Estado = EstadoGeneral.Activo };
        var v3 = new Veterinario { Id = _nextVeterinarioId++, Nombre = "Felipe",  Apellido = "Castro",    Especialidad = "Dermatología Animal", Telefono = "3005556666", Estado = EstadoGeneral.Activo };
        _veterinarios.AddRange(new[] { v1, v2, v3 });

        // --- Mascotas ---
        var m1 = new Mascota { Id = _nextMascotaId++, Nombre = "Max",     PropietarioId = p1.Id, Especie = "Perro", Raza = "Labrador",       FechaNacimiento = new DateTime(2020, 3, 15), Color = "Amarillo", Estado = EstadoGeneral.Activo };
        var m2 = new Mascota { Id = _nextMascotaId++, Nombre = "Luna",    PropietarioId = p1.Id, Especie = "Gato",  Raza = "Siamés",         FechaNacimiento = new DateTime(2021, 7, 20), Color = "Blanco",   Estado = EstadoGeneral.Activo };
        var m3 = new Mascota { Id = _nextMascotaId++, Nombre = "Rocky",   PropietarioId = p2.Id, Especie = "Perro", Raza = "Bulldog Francés",FechaNacimiento = new DateTime(2019, 11, 5), Color = "Atigrado", Estado = EstadoGeneral.Activo };
        var m4 = new Mascota { Id = _nextMascotaId++, Nombre = "Coco",    PropietarioId = p3.Id, Especie = "Ave",   Raza = "Canario",        FechaNacimiento = new DateTime(2022, 1, 10), Color = "Amarillo", Estado = EstadoGeneral.Inactivo };
        _mascotas.AddRange(new[] { m1, m2, m3, m4 });

        // --- Citas ---
        var c1 = new Cita { Id = _nextCitaId++, MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(-5),  Motivo = "Vacunación anual",      Estado = EstadoCita.Completada, Diagnostico = "Paciente en buen estado. Vacunas al día." };
        var c2 = new Cita { Id = _nextCitaId++, MascotaId = m2.Id, VeterinarioId = v2.Id, FechaHora = DateTime.Now.AddDays(2),   Motivo = "Revisión postoperatoria",Estado = EstadoCita.Pendiente,  Diagnostico = null };
        var c3 = new Cita { Id = _nextCitaId++, MascotaId = m3.Id, VeterinarioId = v3.Id, FechaHora = DateTime.Now.AddDays(-1),  Motivo = "Problema de piel",      Estado = EstadoCita.Completada, Diagnostico = "Dermatitis leve. Se receta shampoo medicado." };
        var c4 = new Cita { Id = _nextCitaId++, MascotaId = m1.Id, VeterinarioId = v1.Id, FechaHora = DateTime.Now.AddDays(7),   Motivo = "Control de peso",       Estado = EstadoCita.Pendiente,  Diagnostico = null };
        _citas.AddRange(new[] { c1, c2, c3, c4 });
    }

    // =======================================================================
    // PROPIETARIOS
    // =======================================================================

    public List<Propietario> ObtenerPropietarios() => _propietarios.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToList();

    public Propietario? ObtenerPropietarioPorId(int id) => _propietarios.FirstOrDefault(p => p.Id == id);

    public void AgregarPropietario(Propietario propietario)
    {
        propietario.Id = _nextPropietarioId++;
        _propietarios.Add(propietario);
    }

    public bool ActualizarPropietario(Propietario propietario)
    {
        var existente = _propietarios.FirstOrDefault(p => p.Id == propietario.Id);
        if (existente is null) return false;

        existente.Nombre   = propietario.Nombre;
        existente.Apellido = propietario.Apellido;
        existente.Telefono = propietario.Telefono;
        existente.Correo   = propietario.Correo;
        existente.Estado   = propietario.Estado;
        return true;
    }

    public bool EliminarPropietario(int id)
    {
        var existente = _propietarios.FirstOrDefault(p => p.Id == id);
        if (existente is null) return false;
        _propietarios.Remove(existente);
        return true;
    }

    public bool TieneMascotas(int propietarioId) => _mascotas.Any(m => m.PropietarioId == propietarioId);

    // =======================================================================
    // MASCOTAS
    // =======================================================================

    public List<Mascota> ObtenerMascotas(bool resolverNavegacion = true)
    {
        var lista = _mascotas.OrderBy(m => m.Nombre).ToList();
        if (resolverNavegacion)
            lista.ForEach(m => m.Propietario = ObtenerPropietarioPorId(m.PropietarioId));
        return lista;
    }

    public List<Mascota> ObtenerMascotasPorPropietario(int propietarioId)
    {
        var lista = _mascotas.Where(m => m.PropietarioId == propietarioId).OrderBy(m => m.Nombre).ToList();
        lista.ForEach(m => m.Propietario = ObtenerPropietarioPorId(m.PropietarioId));
        return lista;
    }

    public Mascota? ObtenerMascotaPorId(int id, bool resolverNavegacion = true)
    {
        var mascota = _mascotas.FirstOrDefault(m => m.Id == id);
        if (mascota is not null && resolverNavegacion)
            mascota.Propietario = ObtenerPropietarioPorId(mascota.PropietarioId);
        return mascota;
    }

    public void AgregarMascota(Mascota mascota)
    {
        mascota.Id = _nextMascotaId++;
        _mascotas.Add(mascota);
    }

    public bool ActualizarMascota(Mascota mascota)
    {
        var existente = _mascotas.FirstOrDefault(m => m.Id == mascota.Id);
        if (existente is null) return false;

        existente.Nombre          = mascota.Nombre;
        existente.PropietarioId   = mascota.PropietarioId;
        existente.Especie         = mascota.Especie;
        existente.Raza            = mascota.Raza;
        existente.FechaNacimiento = mascota.FechaNacimiento;
        existente.Color           = mascota.Color;
        existente.Estado          = mascota.Estado;
        return true;
    }

    public bool EliminarMascota(int id)
    {
        var existente = _mascotas.FirstOrDefault(m => m.Id == id);
        if (existente is null) return false;
        _mascotas.Remove(existente);
        return true;
    }

    public bool TieneCitas(int mascotaId) => _citas.Any(c => c.MascotaId == mascotaId);

    // =======================================================================
    // VETERINARIOS
    // =======================================================================

    public List<Veterinario> ObtenerVeterinarios() => _veterinarios.OrderBy(v => v.Apellido).ThenBy(v => v.Nombre).ToList();

    public Veterinario? ObtenerVeterinarioPorId(int id) => _veterinarios.FirstOrDefault(v => v.Id == id);

    public void AgregarVeterinario(Veterinario veterinario)
    {
        veterinario.Id = _nextVeterinarioId++;
        _veterinarios.Add(veterinario);
    }

    public bool ActualizarVeterinario(Veterinario veterinario)
    {
        var existente = _veterinarios.FirstOrDefault(v => v.Id == veterinario.Id);
        if (existente is null) return false;

        existente.Nombre        = veterinario.Nombre;
        existente.Apellido      = veterinario.Apellido;
        existente.Especialidad  = veterinario.Especialidad;
        existente.Telefono      = veterinario.Telefono;
        existente.Estado        = veterinario.Estado;
        return true;
    }

    public bool EliminarVeterinario(int id)
    {
        var existente = _veterinarios.FirstOrDefault(v => v.Id == id);
        if (existente is null) return false;
        _veterinarios.Remove(existente);
        return true;
    }

    // =======================================================================
    // CITAS
    // =======================================================================

    public List<Cita> ObtenerCitas(bool resolverNavegacion = true)
    {
        var lista = _citas.OrderByDescending(c => c.FechaHora).ToList();
        if (resolverNavegacion) ResolverNavegacionCitas(lista);
        return lista;
    }

    public List<Cita> ObtenerCitasPendientes()
    {
        var lista = _citas
            .Where(c => c.Estado == EstadoCita.Pendiente && c.FechaHora >= DateTime.Today)
            .OrderBy(c => c.FechaHora)
            .ToList();
        ResolverNavegacionCitas(lista);
        return lista;
    }

    public Cita? ObtenerCitaPorId(int id, bool resolverNavegacion = true)
    {
        var cita = _citas.FirstOrDefault(c => c.Id == id);
        if (cita is not null && resolverNavegacion)
        {
            cita.Mascota    = ObtenerMascotaPorId(cita.MascotaId, true);
            cita.Veterinario = ObtenerVeterinarioPorId(cita.VeterinarioId);
        }
        return cita;
    }

    public void AgregarCita(Cita cita)
    {
        cita.Id = _nextCitaId++;
        _citas.Add(cita);
    }

    public bool ActualizarCita(Cita cita)
    {
        var existente = _citas.FirstOrDefault(c => c.Id == cita.Id);
        if (existente is null) return false;

        existente.MascotaId    = cita.MascotaId;
        existente.VeterinarioId = cita.VeterinarioId;
        existente.FechaHora    = cita.FechaHora;
        existente.Motivo       = cita.Motivo;
        existente.Estado       = cita.Estado;
        existente.Diagnostico  = cita.Diagnostico;
        return true;
    }

    public bool EliminarCita(int id)
    {
        var existente = _citas.FirstOrDefault(c => c.Id == id);
        if (existente is null) return false;
        _citas.Remove(existente);
        return true;
    }

    // =======================================================================
    // ESTADÍSTICAS para el Dashboard
    // =======================================================================

    public DashboardStats ObtenerEstadisticas() => new DashboardStats
    {
        TotalPropietarios     = _propietarios.Count,
        PropietariosActivos   = _propietarios.Count(p => p.Estado == EstadoGeneral.Activo),
        TotalMascotas         = _mascotas.Count,
        MascotasActivas       = _mascotas.Count(m => m.Estado == EstadoGeneral.Activo),
        TotalVeterinarios     = _veterinarios.Count,
        VeterinariosActivos   = _veterinarios.Count(v => v.Estado == EstadoGeneral.Activo),
        CitasPendientes       = _citas.Count(c => c.Estado == EstadoCita.Pendiente),
        CitasCompletadas      = _citas.Count(c => c.Estado == EstadoCita.Completada),
        CitasCanceladas       = _citas.Count(c => c.Estado == EstadoCita.Cancelada),
        ProximasCitas         = ObtenerCitasPendientes().Take(5).ToList()
    };

    // -----------------------------------------------------------------------
    // Helpers privados
    // -----------------------------------------------------------------------
    private void ResolverNavegacionCitas(List<Cita> citas)
    {
        foreach (var c in citas)
        {
            c.Mascota     = ObtenerMascotaPorId(c.MascotaId, true);
            c.Veterinario = ObtenerVeterinarioPorId(c.VeterinarioId);
        }
    }
}

/// <summary>DTO con las métricas para el Dashboard.</summary>
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
