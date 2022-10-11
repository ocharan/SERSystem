using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.DBContext;
using SER.Entidades;

namespace SER.Pages
{
    public class RegistrarSinodalModel : PageModel
    {
        private readonly MySERContext _context;

        [BindProperty]
        public SinodalDelTrabajo SinodalDelTrabajo { get; set; }

        public RegistrarSinodalModel(MySERContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var sinodalesExistentes = _context.SinodalDelTrabajos.ToList();
                bool sinodalYaExiste = sinodalesExistentes.Any(s => s.CorreoElectronico.Equals(SinodalDelTrabajo.CorreoElectronico));
                if (sinodalYaExiste)
                {
                    TempData["ErrorMessage"] = "Sinodal Existente";
                    return Page();
                }
                else
                {
                    _context.SinodalDelTrabajos.Add(SinodalDelTrabajo);
                    _context.SaveChanges();
                    return RedirectToPage("Sinodales");
                }                              
            }
            return Page();
        }
    }
}