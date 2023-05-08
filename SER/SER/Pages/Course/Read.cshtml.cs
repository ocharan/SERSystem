using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Models.Responses;
using SER.Services;

namespace SER.Pages.Course
{
  [Authorize(Roles = "Administrador")]
  public class ReadModel : PageModel
  {
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;
    private readonly IProfessorService _professorService;
    // private readonly I
    [BindProperty]
    public CourseDto course { get; set; } = null!;
    [BindProperty]
    public IFormFile? fileUpload { get; set; }

    public ReadModel(ICourseService courseService, IStudentService studentService, IProfessorService professorService)
    {
      _courseService = courseService;
      _studentService = studentService;
      _professorService = professorService;
    }

    public async Task<IActionResult> OnGet(string courseId)
    {
      try
      {
        int.TryParse(courseId, out int id);
        course = await _courseService.GetCourse(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Course/Index");
      }

      return Page();
    }

    public async Task<JsonResult> OnPostCreateCourseRegistrations([FromBody] List<CourseRegistrationDto> registrations)
    {
      try
      {
        var response = await _courseService.CreateCourseRegistrations(registrations);
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
      }

      return new JsonResult("ok");
    }

    public class ProfessorAssignmentRequest
    {
      public int CourseId { get; set; }
      public int ProfessorId { get; set; }
    }

    public async Task<JsonResult> OnPostCreateProfessorAssignment([FromBody] ProfessorAssignmentRequest request)
    {
      try
      {
        var response = await _courseService.CreateProfessorAssignment(request.CourseId, request.ProfessorId);
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
      }

      return new JsonResult("ok");
    }
  }
}