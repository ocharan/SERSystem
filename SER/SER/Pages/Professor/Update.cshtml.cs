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
  public class UpdateModel : PageModel
  {
    private readonly IProfessorService _professorService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public ProfessorDto professor { get; set; } = null!;

    public UpdateModel(IProfessorService professorService) { _professorService = professorService; }

    public async Task<IActionResult> OnGet(string professorId)
    {
      try
      {
        int.TryParse(professorId, out int id);
        professor = await _professorService.GetProfessor(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Professor/Index");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostUpdateProfessor()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _professorService.UpdateProfessor(professor);

        if (!response.IsSuccess)
        {
          HandleModelErrors(response.Errors);

          return Page();
        }

        TempData["MessageSuccess"] = EStatusCodes.Ok;
      }
      catch (Exception ex) when (ex is OperationCanceledException || ex is NullReferenceException)
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