using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Citas;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;

    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Cita Cita { get; set; } = new() { FechaHora = DateTime.Now.AddHours(1) };

    public SelectList MascotasSelectList     { get; set; } = default!;
    public SelectList VeterinariosSelectList { get; set; } = default!;

    public IActionResult OnGet(int? mascotaId)
    {
        CargarSelectLists();
        if (mascotaId.HasValue)
            Cita.MascotaId = mascotaId.Value;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            CargarSelectLists();
            return Page();
        }

        _svc.AgregarCita(Cita);
        TempData["Exito"] = "Cita agendada correctamente.";
        return RedirectToPage("Index");
    }

    private void CargarSelectLists()
    {
        // Mascotas activas con nombre + especie para facilitar selección
        var mascotas = _svc.ObtenerMascotas()
                           .Where(m => m.Estado == EstadoGeneral.Activo)
                           .Select(m => new
                           {
                               m.Id,
                               Descripcion = $"{m.Nombre} ({m.Especie}) – {m.Propietario?.NombreCompleto}"
                           })
                           .ToList();
        MascotasSelectList = new SelectList(mascotas, "Id", "Descripcion");

        // Veterinarios activos
        var veterinarios = _svc.ObtenerVeterinarios()
                               .Where(v => v.Estado == EstadoGeneral.Activo)
                               .Select(v => new
                               {
                                   v.Id,
                                   Descripcion = $"{v.NombreCompleto} – {v.Especialidad}"
                               })
                               .ToList();
        VeterinariosSelectList = new SelectList(veterinarios, "Id", "Descripcion");
    }
}
