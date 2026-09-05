using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Veterinarios;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;

    public IndexModel(VeterinariaService svc) => _svc = svc;

    [BindProperty(SupportsGet = true)] public string? Busqueda    { get; set; }
    [BindProperty(SupportsGet = true)] public string? FiltroEstado { get; set; }

    public List<Veterinario> Veterinarios { get; set; } = new();

    public void OnGet()
    {
        var lista = _svc.ObtenerVeterinarios();

        if (!string.IsNullOrWhiteSpace(Busqueda))
        {
            var q = Busqueda.Trim().ToLower();
            lista = lista.Where(v =>
                v.Nombre.ToLower().Contains(q)       ||
                v.Apellido.ToLower().Contains(q)     ||
                v.Especialidad.ToLower().Contains(q)
            ).ToList();
        }

        if (!string.IsNullOrEmpty(FiltroEstado) &&
            Enum.TryParse<EstadoGeneral>(FiltroEstado, out var estado))
            lista = lista.Where(v => v.Estado == estado).ToList();

        Veterinarios = lista;
    }
    public int ContarCitasPendientes(int veterinarioId) =>
        _svc.ObtenerCitas(false)
            .Count(c => c.VeterinarioId == veterinarioId && c.Estado == EstadoCita.Pendiente);
}
