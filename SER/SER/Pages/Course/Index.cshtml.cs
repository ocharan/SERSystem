using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity;
using Microsoft.AspNetCore.Mvc;
using SER.Configuration;

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
    public PaginatedList<CourseDto> courses = null!;
    public int OpenCourses { get; set; }
    public int ClosedCourses { get; set; }

    public Index(ICourseService courseService, IConfiguration configuration)
    {
      _courseService = courseService;
      Configuration = configuration;
    }

    public async Task OnGet(string sortOrder, string currentSearch, string searchString, int? pageIndex, string currentFilter)
    {
      if (TempData["MessageSuccess"] != null) { ViewData["MessageSuccess"] = TempData["MessageSuccess"]; }

      if (TempData["MessageError"] != null) { ViewData["MessageError"] = TempData["MessageError"]; }

      CurrentSort = sortOrder;
      NameSort = String.IsNullOrEmpty(sortOrder) ? "descendant-name" : "";
      int PAGE_SIZE = Configuration.GetValue("PageSize", 10);

      if (!String.IsNullOrEmpty(searchString)) { pageIndex = 1; }
      else { searchString = currentSearch; }

      CurrentSearch = searchString;
      CurrentSort = sortOrder ?? CurrentSort;
      CurrentFilter = currentFilter ?? CurrentFilter;

      var auxiliaryCourses = _courseService.GetAllCourses();

      auxiliaryCourses = !String.IsNullOrEmpty(currentFilter)
        ? FilterCourses(auxiliaryCourses, currentFilter)
        : FilterCourses(auxiliaryCourses);

      if (!String.IsNullOrEmpty(searchString))
      {
        auxiliaryCourses = auxiliaryCourses.Where(course =>
          course.Name.Contains(searchString) || (Convert.ToString(course.Nrc)).Contains(searchString)
        );
      }

      auxiliaryCourses = String.Equals(sortOrder, "descendant-name")
        ? auxiliaryCourses.OrderByDescending(course => course.Name)
        : auxiliaryCourses.OrderBy(course => course.Name);

      courses = await PaginatedList<CourseDto>.CreateAsync(
        auxiliaryCourses, pageIndex ?? 1, PAGE_SIZE
      );
    }

    public IQueryable<CourseDto> FilterCourses(IQueryable<CourseDto> courses, string filter = "default")
    {
      OpenCourses = courses
        .Where(course => course.IsOpen)
        .Count();

      ClosedCourses = courses
        .Where(course => !course.IsOpen)
        .Count();

      if (filter.Equals("open")) { courses = courses.Where(course => course.IsOpen).AsQueryable(); }
      if (filter.Equals("closed")) { courses = courses.Where(course => !course.IsOpen).AsQueryable(); }

      return courses;
    }
  }
}