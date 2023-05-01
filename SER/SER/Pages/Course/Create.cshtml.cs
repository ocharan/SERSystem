using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using SER.Models.Responses;

namespace SER.Pages.Course
{
  [RequestSizeLimit(1073741824)]
  public class CreateModel : PageModel
  {
    private readonly ICourseService _courseService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public CourseDto course { get; set; } = null!;
    [BindProperty]
    public IFormFile? fileUpload { get; set; }

    public CreateModel(ICourseService courseService) { _courseService = courseService; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAddCourse()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        if (fileUpload != null) { response = await _courseService.CreateCourse(course, fileUpload); }
        else { response = await _courseService.CreateCourse(course); }
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      if (!response.IsSuccess)
      {
        foreach (var error in response.Errors)
        {
          ModelState.AddModelError(error.FieldName, error.Message);
        }

        return Page();
      }
      else
      {
        TempData["MessageSuccess"] = "Se ha verificado la información del curso y se ha registrado correctamente.";
      }

      return RedirectToPage("Index");
    }
  }
}