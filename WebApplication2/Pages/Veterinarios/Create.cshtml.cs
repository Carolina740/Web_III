using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Veterinarios;

public class CreateModel : PageModel
{
    private readonly VeterinariaService _svc;

    public CreateModel(VeterinariaService svc) => _svc = svc;

    [BindProperty]
    public Veterinario Veterinario { get; set; } = new();

    public IActionResult OnGet() => Page();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _svc.AgregarVeterinario(Veterinario);
        TempData["Exito"] = $"Veterinario '{Veterinario.NombreCompleto}' registrado correctamente.";
        return RedirectToPage("Index");
    }
}
