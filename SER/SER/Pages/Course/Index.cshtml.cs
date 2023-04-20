using ContosoUniversity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using SER.Services;

namespace SER.Pages.Course
{
  [Authorize(Roles = "Administrador")]
  public class Index : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly ICourseService _courseService;
    public string? NameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    public int OpenCount { get; set; }
    public int ClosedCount { get; set; }
    public PaginatedList<CourseDto> courses = null!;

    public Index(ICourseService courseService, IConfiguration configuration)
    {
      _courseService = courseService;
      Configuration = configuration;
    }

    public async Task OnGet(string sortOrder, string currentSearch, string searchString, int? pageIndex, string currentFilter)
    {
      CurrentSort = sortOrder;
      NameSort = String.IsNullOrEmpty(sortOrder) ? "descendant-name" : "";

      if (!String.IsNullOrEmpty(searchString)) { pageIndex = 1; }
      else { searchString = currentSearch; }

      CurrentSearch = searchString;
      CurrentSort = sortOrder ?? CurrentSort;
      CurrentFilter = currentFilter ?? CurrentFilter;

      var auxiliaryCourses = !String.IsNullOrWhiteSpace(currentFilter)
        ? _courseService.GetAllCourses(currentFilter)
        : _courseService.GetAllCourses("default");

      OpenCount = auxiliaryCourses.OpenCount;
      ClosedCount = auxiliaryCourses.ClosedCount;

      if (!String.IsNullOrEmpty(searchString))
      {
        auxiliaryCourses.Courses = auxiliaryCourses.Courses.Where(course =>
          course.Name.Contains(searchString) || course.Nrc.ToString().Contains(searchString)
        );
      }

      int PAGE_SIZE = Configuration.GetValue("PageSize", 10);

      auxiliaryCourses.Courses = String.Equals(sortOrder, "descendant-name")
        ? auxiliaryCourses.Courses.OrderByDescending(course => course.Name)
        : auxiliaryCourses.Courses.OrderBy(course => course.Name);

      courses = await PaginatedList<CourseDto>.CreateAsync(
        auxiliaryCourses.Courses.AsNoTracking(), pageIndex ?? 1, 10
      );
    }
  }
}