using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using Microsoft.AspNetCore.Authorization;
using SER.Models.Responses;
using SER.Models.Enums;

namespace SER.Pages.Student
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class UpdateModel : PageModel
  {
    private readonly IStudentService _studentService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public StudentDto student { get; set; } = null!;

    public UpdateModel(IStudentService studentService) { _studentService = studentService; }

    public async Task<IActionResult> OnGet(string studentId)
    {
      try
      {
        int.TryParse(studentId, out int id);
        student = await _studentService.GetStudent(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Student/Index");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostUpdateStudent()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _studentService.UpdateStudent(student);
      }
      catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
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

      TempData["MessageSuccess"] = EStatusCodes.Ok;

      return RedirectToPage("/Student/Index");
    }
  }
}