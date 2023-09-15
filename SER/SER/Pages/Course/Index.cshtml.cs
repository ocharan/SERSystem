using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SER.Configuration;
using SER.Models.Enums;
using SER.Models.DB;
using System.Security.Claims;

namespace SER.Pages.Course
{
  [Authorize(Roles = (nameof(ERoles.Administrator)) + "," + (nameof(ERoles.Professor)))]
  public class IndexModel : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly ICourseService _courseService;
    public string? NameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    public int OpenCourses { get; set; }
    public int ClosedCourses { get; set; }
    public PaginatedList<CourseDto> courses { get; set; } = null!;

    public IndexModel(ICourseService courseService, IConfiguration configuration)
    {
      _courseService = courseService;
      Configuration = configuration;
    }

    public async Task OnGet(string sortOrder, string currentSearch, string searchString,
      int? pageIndex, string currentFilter)
    {
      ViewData["MessageSuccess"] = TempData["MessageSuccess"];
      ViewData["MessageError"] = TempData["MessageError"];

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

      auxiliaryCourses = SearchCourse(auxiliaryCourses, searchString);

      auxiliaryCourses = String.Equals(sortOrder, "descendant-name")
        ? auxiliaryCourses.OrderByDescending(course => course.Name)
        : auxiliaryCourses.OrderBy(course => course.Name);

      courses = await PaginatedList<CourseDto>
        .CreateAsync(auxiliaryCourses.AsNoTracking(), pageIndex ?? 1, PAGE_SIZE);

      // courses.ForEach(async course =>
      //   { course.Period = await FormatCoursePeriod(course.Period); });
    }

    public async Task<FileResult> OnPostExportSpreadsheetData()
    {
      var courses = await FilterCourses().ToListAsync();
      XLWorkbook workbook = new XLWorkbook();
      IXLWorksheet worksheet = workbook.Worksheets.Add("Cursos");

      worksheet.Cell(1, 1).Value = "Abierto";
      worksheet.Cell(1, 2).Value = "NRC";
      worksheet.Cell(1, 3).Value = "Nombre";
      worksheet.Cell(1, 4).Value = "Periodo";
      worksheet.Cell(1, 5).Value = "Sección";
      worksheet.Cell(1, 6).Value = "Profesor asignado";

      for (int i = 0; i < courses.Count; i++)
      {
        worksheet.Cell(i + 2, 1).Value = courses[i].IsOpen ? "Si" : "No";
        worksheet.Cell(i + 2, 2).Value = courses[i].Nrc;
        worksheet.Cell(i + 2, 3).Value = courses[i].Name;
        // worksheet.Cell(i + 2, 4).Value = await FormatCoursePeriod(courses[i].Period);
        worksheet.Cell(i + 2, 4).Value = courses[i].Period;
        worksheet.Cell(i + 2, 5).Value = courses[i].Section;

        var professor = courses[i].Professor;

        string professorName = professor != null
          ? courses[i].Professor!.FullName
          : "Sin asignar";

        worksheet.Cell(i + 2, 6).Value = professorName;
      }

      using (var stream = new MemoryStream())
      {
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        return File
        (
          content,
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Cursos.xlsx"
        );
      }
    }

    private IQueryable<CourseDto> FilterCourses(string filter = "default")
    {
      string userRole = User.FindFirst(ClaimTypes.Role)!.Value;

      var courses = userRole.Equals(nameof(ERoles.Administrator))
        ? _courseService.GetAllCourses()
        : _courseService.GetProfessorCourses(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

      OpenCourses = courses
        .Where(course => course.IsOpen)
        .Count();

      ClosedCourses = courses.Count() - OpenCourses;

      if (filter.Equals("open")) { courses = courses.Where(course => course.IsOpen); }

      if (filter.Equals("closed")) { courses = courses.Where(course => !course.IsOpen); }

      return courses;
    }

    private Task<string> FormatCoursePeriod(string period)
    {
      string[] dates = period.Split("_");
      DateTime startDate = DateTime.ParseExact(dates[0], "MMMMyyyy", null);
      DateTime endDate = DateTime.ParseExact(dates[1], "MMMMyyyy", null);

      string formattedPeriod = $"{startDate.ToString("MMMM yyyy")} - {endDate.ToString("MMMM yyyy")}";

      return Task.FromResult(formattedPeriod);
    }

    private IQueryable<CourseDto> SearchCourse(IQueryable<CourseDto> courses, string search)
    {
      if (!String.IsNullOrEmpty(search))
      {
        courses = courses.Where(course => course.Name.Contains(search)
          || (Convert.ToString(course.Nrc)).Contains(search));
      }

      return courses;
    }
  }
}