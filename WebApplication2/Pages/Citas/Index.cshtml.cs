using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Citas;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;

    public IndexModel(VeterinariaService svc) => _svc = svc;

    [BindProperty(SupportsGet = true)] public string?   FiltroEstado      { get; set; }
    [BindProperty(SupportsGet = true)] public int?      FiltroVeterinario { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? FiltroDesde       { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? FiltroHasta       { get; set; }

    public List<Cita>        Citas        { get; set; } = new();
    public List<Veterinario> Veterinarios { get; set; } = new();

    public void OnGet()
    {
        Veterinarios = _svc.ObtenerVeterinarios();
        var lista    = _svc.ObtenerCitas();

        if (!string.IsNullOrEmpty(FiltroEstado) &&
            Enum.TryParse<EstadoCita>(FiltroEstado, out var estado))
            lista = lista.Where(c => c.Estado == estado).ToList();

        if (FiltroVeterinario.HasValue && FiltroVeterinario > 0)
            lista = lista.Where(c => c.VeterinarioId == FiltroVeterinario).ToList();

        if (FiltroDesde.HasValue)
            lista = lista.Where(c => c.FechaHora.Date >= FiltroDesde.Value.Date).ToList();

        if (FiltroHasta.HasValue)
            lista = lista.Where(c => c.FechaHora.Date <= FiltroHasta.Value.Date).ToList();

        Citas = lista;
    }
}
