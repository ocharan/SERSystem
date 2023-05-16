using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Models.DTO;
using SER.Services;
using SER.Configuration;
using SER.Models.Enums;

namespace SER.Pages.Student
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class Read : PageModel
  {
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;
    [BindProperty]
    public StudentDto student { get; set; } = null!;
    public List<CourseRegistrationDto> StudentCourses { get; set; } = null!;

    public Read(IStudentService studentService, ICourseService courseService)
    {
      _studentService = studentService;
      _courseService = courseService;
    }

    public async Task<IActionResult> OnGet(string studentId)
    {
      try
      {
        int.TryParse(studentId, out int id);
        student = await _studentService.GetStudent(id);
        StudentCourses = await _courseService.GetStudentCourses(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Student/Index");
      }

      return Page();
    }
  }
}