using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Models.Responses;
using SER.Services;
using SER.Models.Enums;

namespace SER.Pages.Professor
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class CreateModel : PageModel
  {
    private readonly IProfessorService _professorService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public ProfessorDto professor { get; set; } = null!;

    public CreateModel(IProfessorService professorService) { _professorService = professorService; }

    public IActionResult OnGet() { return Page(); }

    public async Task<IActionResult> OnPostCreateProfessor()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _professorService.CreateProfessor(professor);

        if (!response.IsSuccess)
        {
          HandleModelErrors(response.Errors);

          return Page();
        }

        TempData["MessageSuccess"] = EStatusCodes.Created;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/Professor/Index");
    }

    private void HandleModelErrors(List<FieldError> errors)
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError(error.FieldName, error.Message);
      }
    }
  }
}