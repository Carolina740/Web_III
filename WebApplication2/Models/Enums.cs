namespace WebApplication2.Models;

/// <summary>
/// Estado general para Propietarios, Mascotas y Veterinarios.
/// </summary>
public enum EstadoGeneral
{
    Activo = 1,
    Inactivo = 0
}

/// <summary>
/// Estado específico para el ciclo de vida de una Cita.
/// </summary>
public enum EstadoCita
{
    Pendiente = 0,
    Completada = 1,
    Cancelada = 2
}
