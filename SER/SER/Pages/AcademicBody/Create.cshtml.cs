using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using SER.Services.Contracts;

namespace SER.Pages.AcademicBody
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  [RequestSizeLimit(1073741824)]
  public class CreateModel : PageModel
  {
    private readonly IAcademicBodyService _academicBodyService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public AcademicBodyDto academicBody { get; set; } = null!;

    public CreateModel(IAcademicBodyService academicBodyService) { _academicBodyService = academicBodyService; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAddAcademicBody()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _academicBodyService.CreateAcademicBody(academicBody);

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

      return RedirectToPage("Index");
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