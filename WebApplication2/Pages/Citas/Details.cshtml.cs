using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Citas;

public class DetailsModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DetailsModel(VeterinariaService svc) => _svc = svc;

    public Cita? Cita { get; set; }

    public IActionResult OnGet(int id)
    {
        Cita = _svc.ObtenerCitaPorId(id, resolverNavegacion: true);
        if (Cita is null)
        {
            TempData["Error"] = "Cita no encontrada.";
            return RedirectToPage("Index");
        }
        return Page();
    }
    public IActionResult OnPostCambiarEstado(int id, string estado)
    {
        var cita = _svc.ObtenerCitaPorId(id, resolverNavegacion: false);
        if (cita is null)
        {
            TempData["Error"] = "Cita no encontrada.";
            return RedirectToPage("Index");
        }

        if (!Enum.TryParse<EstadoCita>(estado, out var nuevoEstado))
        {
            TempData["Error"] = "Estado no válido.";
            return RedirectToPage("Details", new { id });
        }

        cita.Estado = nuevoEstado;
        _svc.ActualizarCita(cita);

        TempData["Exito"] = $"Estado de la cita actualizado a '{nuevoEstado}'.";
        return RedirectToPage("Details", new { id });
    }
}
