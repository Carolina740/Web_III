using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Propietarios;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;

    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Propietario Propietario { get; set; } = new();

    public IActionResult OnGet() => Page();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _svc.AgregarPropietario(Propietario);
        TempData["Exito"] = $"Propietario '{Propietario.NombreCompleto}' registrado correctamente.";
        return RedirectToPage("Index");
    }
}
