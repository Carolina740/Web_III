using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Propietarios;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;

    public IndexModel(VeterinariaService svc) => _svc = svc;

    [BindProperty(SupportsGet = true)]
    public string? Busqueda { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FiltroEstado { get; set; }

    public List<Propietario> Propietarios { get; set; } = new();

    public void OnGet()
    {
        var lista = _svc.ObtenerPropietarios();

        if (!string.IsNullOrWhiteSpace(Busqueda))
        {
            var q = Busqueda.Trim().ToLower();
            lista = lista.Where(p =>
                p.Nombre.ToLower().Contains(q)  ||
                p.Apellido.ToLower().Contains(q) ||
                p.Correo.ToLower().Contains(q)
            ).ToList();
        }

        if (!string.IsNullOrEmpty(FiltroEstado) &&
            Enum.TryParse<EstadoGeneral>(FiltroEstado, out var estado))
        {
            lista = lista.Where(p => p.Estado == estado).ToList();
        }

        Propietarios = lista;
    }
    public int ContarMascotas(int propietarioId) =>
        _svc.ObtenerMascotasPorPropietario(propietarioId).Count;
}
