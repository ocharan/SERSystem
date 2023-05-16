using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Models.Responses;
using SER.Services;
using SER.Models.Enums;

namespace SER.Pages.Student
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class CreateModel : PageModel
  {
    private readonly IStudentService _studentService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public StudentDto student { get; set; } = null!;

    public CreateModel(IStudentService studentService) { _studentService = studentService; }

    public IActionResult OnGet() { return Page(); }

    public async Task<IActionResult> OnPostCreateStudent()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _studentService.CreateStudent(student);
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      if (response.Errors.Count > 0 && !response.IsSuccess)
      {
        foreach (var error in response.Errors)
        {
          ModelState.AddModelError(error.FieldName, error.Message);
        }

        return Page();
      }

      TempData["MessageSuccess"] = EStatusCodes.Created;

      return RedirectToPage("/Student/Index");
    }
  }
}