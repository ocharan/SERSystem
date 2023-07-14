using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Services;
using SER.Models.Enums;
using System.Net;

namespace SER.Pages.Course
{
  [Authorize(Roles = (nameof(ERoles.Administrator)) + "," + (nameof(ERoles.Professor)))]
  public class ReadModel : PageModel
  {
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;
    private readonly IProfessorService _professorService;
    [BindProperty]
    public CourseDto course { get; set; } = null!;
    [BindProperty]
    public IFormFile? fileUpload { get; set; }
    public string? FilePath { get; set; }
    public class ProfessorAssignmentRequest
    {
      public int CourseId { get; set; }
      public int ProfessorId { get; set; }
    }

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

        if (course.FileId != null)
        {
          FilePath = (await _courseService.GetCourseFile(course.FileId.Value)).Path
            .Split("/")
            .Last();
        }

        course.Period = await FormatCoursePeriod(course.Period);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Course/Index");
      }

      return Page();
    }

    public async Task<FileResult> OnGetCourseFile(string courseId)
    {
      bool result = int.TryParse(courseId, out int courseIdFind);

      if (!result) { return null!; }

      try
      {
        FilePath = (await _courseService.GetCourseFile(courseIdFind)).Path;
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
      }

      string contentType = "application/pdf";

      return File(FilePath!, contentType);
    }

    public async Task<JsonResult> OnPostCreateCourseRegistrations
      ([FromBody] List<CourseRegistrationDto> registrations)
    {
      try
      {
        await _courseService.CreateCourseRegistrations(registrations);

        return new JsonResult(new { message = "Success Message" })
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult(new { message = "Error Message" })
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }

    public async Task<JsonResult> OnPostCreateProfessorAssignment
      ([FromBody] ProfessorAssignmentRequest request)
    {
      try
      {
        await _courseService.CreateProfessorAssignment(request.CourseId, request.ProfessorId);

        return new JsonResult(new { message = "Success Message" })
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult(new { message = "Error Message" })
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }

    public async Task<IActionResult> OnPostDeleteCourseRegistration
      ([FromBody] int registrationId)
    {
      try
      {
        await _courseService.DeleteCourseRegistration(registrationId);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }

    private Task<string> FormatCoursePeriod(string period)
    {
      string[] dates = period.Split("_");
      DateTime startDate = DateTime.ParseExact(dates[0], "MMMMyyyy", null);
      DateTime endDate = DateTime.ParseExact(dates[1], "MMMMyyyy", null);

      string formattedPeriod = $"{startDate.ToString("MMMM yyyy")} - {endDate.ToString("MMMM yyyy")}";

      return Task.FromResult(formattedPeriod);
    }
  }
}