using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using ContosoUniversity;
using Microsoft.AspNetCore.Mvc;
using SER.Configuration;

namespace SER.Pages.Course
{
  [Authorize(Roles = "Administrador")]
  public class IndexModel : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly ICourseService _courseService;
    public string? NameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    public PaginatedList<CourseDto> courses = null!;
    public int OpenCourses { get; set; }
    public int ClosedCourses { get; set; }

    public IndexModel(ICourseService courseService, IConfiguration configuration)
    {
      _courseService = courseService;
      Configuration = configuration;
    }

    public async Task OnGet(string sortOrder, string currentSearch, string searchString, int? pageIndex, string currentFilter)
    {
      CheckStatusCode();
      CurrentSort = sortOrder;
      NameSort = String.IsNullOrEmpty(sortOrder) ? "descendant-name" : "";
      int PAGE_SIZE = Configuration.GetValue("PageSize", 10);

      if (!String.IsNullOrEmpty(searchString)) { pageIndex = 1; }
      else { searchString = currentSearch; }

      CurrentSearch = searchString;
      CurrentFilter = currentFilter ?? CurrentFilter;

      var auxiliaryCourses = !String.IsNullOrEmpty(currentFilter)
        ? FilterCourses(currentFilter)
        : FilterCourses();

      if (!String.IsNullOrEmpty(searchString))
      {
        auxiliaryCourses = auxiliaryCourses
          .Where(course => course.Name.Contains(searchString)
            || (Convert.ToString(course.Nrc)).Contains(searchString)
        );
      }

      auxiliaryCourses = String.Equals(sortOrder, "descendant-name")
        ? auxiliaryCourses.OrderByDescending(course => course.Name)
        : auxiliaryCourses.OrderBy(course => course.Name);

      courses = await PaginatedList<CourseDto>.CreateAsync(
        auxiliaryCourses, pageIndex ?? 1, PAGE_SIZE
      );
    }

    public void CheckStatusCode()
    {
      if (TempData["MessageSuccess"] != null)
      {
        ViewData["MessageSuccess"] = TempData["MessageSuccess"];
      }

      if (TempData["MessageError"] != null)
      {
        ViewData["MessageError"] = TempData["MessageError"];
      }
    }

    public IQueryable<CourseDto> FilterCourses(string filter = "default")
    {
      var courses = _courseService.GetAllCourses();

      OpenCourses = courses
        .Where(course => course.IsOpen)
        .Count();

      ClosedCourses = courses.Count() - OpenCourses;

      if (filter.Equals("open")) { courses = courses.Where(course => course.IsOpen); }

      if (filter.Equals("closed")) { courses = courses.Where(course => !course.IsOpen); }

      return courses;
    }
  }
}