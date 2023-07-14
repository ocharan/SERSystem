using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Models.Enums;
using SER.Models.Responses;
using SER.Services.Contracts;

namespace SER.Pages.AcademicBody
{
  public class UpdateModel : PageModel
  {
    private readonly IAcademicBodyService _academicBodyService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public AcademicBodyDto academicBody { get; set; } = null!;

    public UpdateModel(IAcademicBodyService academicBodyService) { _academicBodyService = academicBodyService; }

    public async Task<IActionResult> OnGet(string academicBodyId)
    {
      try
      {
        int.TryParse(academicBodyId, out int id);
        academicBody = await _academicBodyService.GetAcademicBody(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/AcademicBody/Index");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostUpdateAcademicBody()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _academicBodyService.UpdateAcademicBody(academicBody);

        if (!response.IsSuccess)
        {
          HandleModelErrors(response.Errors);

          return Page();
        }

        TempData["MessageSuccess"] = EStatusCodes.Ok;
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/AcademicBody/Index");
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