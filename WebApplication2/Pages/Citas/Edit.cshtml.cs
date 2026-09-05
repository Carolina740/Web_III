using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Citas;

public class EditModel : PageModel
{
    private readonly VeterinariaService _svc;

    public EditModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Cita? Cita { get; set; }

    public SelectList MascotasSelectList     { get; set; } = default!;
    public SelectList VeterinariosSelectList { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        Cita = _svc.ObtenerCitaPorId(id, resolverNavegacion: false);
        if (Cita is null)
        {
            TempData["Error"] = "Cita no encontrada.";
            return RedirectToPage("Index");
        }
        CargarSelectLists();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            CargarSelectLists();
            return Page();
        }
        if (Cita is null) return RedirectToPage("Index");

        var ok = _svc.ActualizarCita(Cita);
        if (!ok)
        {
            TempData["Error"] = "No se pudo actualizar la cita.";
            return RedirectToPage("Index");
        }

        TempData["Exito"] = "Cita actualizada correctamente.";
        return RedirectToPage("Details", new { id = Cita.Id });
    }

    private void CargarSelectLists()
    {
        var mascotas = _svc.ObtenerMascotas()
                           .Select(m => new
                           {
                               m.Id,
                               Descripcion = $"{m.Nombre} ({m.Especie}) – {m.Propietario?.NombreCompleto}"
                           }).ToList();
        MascotasSelectList = new SelectList(mascotas, "Id", "Descripcion");

        var veterinarios = _svc.ObtenerVeterinarios()
                               .Select(v => new
                               {
                                   v.Id,
                                   Descripcion = $"{v.NombreCompleto} – {v.Especialidad}"
                               }).ToList();
        VeterinariosSelectList = new SelectList(veterinarios, "Id", "Descripcion");
    }
}
