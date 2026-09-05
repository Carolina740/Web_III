using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Mascotas;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;

    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Mascota Mascota { get; set; } = new() { FechaNacimiento = DateTime.Today.AddYears(-1) };

    public SelectList PropietariosSelectList { get; set; } = default!;

    public IActionResult OnGet(int? propietarioId)
    {
        CargarSelectLists();
        if (propietarioId.HasValue)
            Mascota.PropietarioId = propietarioId.Value;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            CargarSelectLists();
            return Page();
        }

        _svc.AgregarMascota(Mascota);
        TempData["Exito"] = $"Mascota '{Mascota.Nombre}' registrada correctamente.";
        return RedirectToPage("Index");
    }

    private void CargarSelectLists()
    {
        var propietarios = _svc.ObtenerPropietarios()
                               .Where(p => p.Estado == EstadoGeneral.Activo)
                               .ToList();
        PropietariosSelectList = new SelectList(propietarios, "Id", "NombreCompleto");
    }
}
