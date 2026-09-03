using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Mascotas;

public class IndexModel : PageModel
{
    private readonly VeterinariaService _svc;

    public IndexModel(VeterinariaService svc) => _svc = svc;

    [BindProperty(SupportsGet = true)] public string?  Busqueda         { get; set; }
    [BindProperty(SupportsGet = true)] public int?     FiltroPropietario { get; set; }
    [BindProperty(SupportsGet = true)] public string?  FiltroEstado     { get; set; }

    public List<Mascota>     Mascotas     { get; set; } = new();
    public List<Propietario> Propietarios { get; set; } = new();

    public void OnGet()
    {
        Propietarios = _svc.ObtenerPropietarios();
        var lista    = _svc.ObtenerMascotas();

        if (!string.IsNullOrWhiteSpace(Busqueda))
        {
            var q = Busqueda.Trim().ToLower();
            lista = lista.Where(m =>
                m.Nombre.ToLower().Contains(q)  ||
                m.Especie.ToLower().Contains(q) ||
                m.Raza.ToLower().Contains(q)
            ).ToList();
        }

        if (FiltroPropietario.HasValue && FiltroPropietario > 0)
            lista = lista.Where(m => m.PropietarioId == FiltroPropietario).ToList();

        if (!string.IsNullOrEmpty(FiltroEstado) &&
            Enum.TryParse<EstadoGeneral>(FiltroEstado, out var estado))
            lista = lista.Where(m => m.Estado == estado).ToList();

        Mascotas = lista;
    }
}
