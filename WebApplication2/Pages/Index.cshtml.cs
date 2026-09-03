using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;

    public IndexModel(VeterinariaService svc) => _svc = svc;

    public DashboardStats    Stats               { get; set; } = default!;
    public List<Propietario> PropietariosActivos { get; set; } = new();
    public List<Mascota>     MascotasActivas     { get; set; } = new();

    public void OnGet()
    {
        Stats = _svc.ObtenerEstadisticas();

        PropietariosActivos = _svc.ObtenerPropietarios()
                                  .Where(p => p.Estado == EstadoGeneral.Activo)
                                  .Take(5)
                                  .ToList();

        MascotasActivas = _svc.ObtenerMascotas()
                              .Where(m => m.Estado == EstadoGeneral.Activo)
                              .Take(8)
                              .ToList();
    }

    /// <summary>Cuenta mascotas de un propietario (para la tabla del dashboard).</summary>
    public int ContarMascotas(int propietarioId) =>
        _svc.ObtenerMascotasPorPropietario(propietarioId).Count;
}
