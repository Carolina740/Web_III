using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Citas;

public class DeleteModel : PageModel
{
    private readonly VeterinariaService _svc;

    public DeleteModel(VeterinariaService svc) => _svc = svc;

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

    public IActionResult OnPost(int id)
    {
        var cita   = _svc.ObtenerCitaPorId(id, resolverNavegacion: false);
        var nombre = $"Cita #{id}";

        _svc.EliminarCita(id);
        TempData["Exito"] = $"{nombre} eliminada correctamente.";
        return RedirectToPage("Index");
    }
}
