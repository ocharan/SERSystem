using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Models.DTO;
using SER.Services;
using SER.Configuration;
using SER.Models.Enums;

namespace SER.Pages.Professor
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class ReadModel : PageModel
  {
    private readonly IProfessorService _professorService;
    private readonly ICourseService _courseService;
    [BindProperty]
    public ProfessorDto professor { get; set; } = null!;
    public List<CourseDto> ProfessorCourses { get; set; } = null!;

    public ReadModel(IProfessorService professorService, ICourseService courseService)
    {
      _professorService = professorService;
      _courseService = courseService;
    }

    public async Task<IActionResult> OnGet(string professorId)
    {
      try
      {
        int.TryParse(professorId, out int id);
        professor = await _professorService.GetProfessor(id);

        professor.Courses!.ForEach(async course =>
        {
          course.Period = await FormatCoursePeriod(course.Period);
        });
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Professor/Index");
      }

      return Page();
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