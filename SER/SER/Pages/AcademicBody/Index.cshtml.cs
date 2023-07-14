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
using SER.Services.Contracts;

namespace SER.Pages.AcademicBody
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class IndexModel : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly IAcademicBodyService _academicBodyService;
    public string? FullnameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    // public int AssignedStudents { get; set; }
    // public int UnassignedStudents { get; set; }
    public PaginatedList<AcademicBodyDto> academicBodies { get; set; } = null!;

    public IndexModel(IAcademicBodyService academicBodyService, IConfiguration configuration)
    {
      _academicBodyService = academicBodyService;
      Configuration = configuration;
    }

    public async Task OnGet(string sortOrder, string currentSearch, string searchString,
      int? pageIndex, string currentFilter)
    {
      ViewData["MessageSuccess"] = TempData["MessageSuccess"];
      ViewData["MessageError"] = TempData["MessageError"];

      CurrentSort = sortOrder;
      FullnameSort = String.IsNullOrEmpty(sortOrder) ? "descendant-fullname" : "";
      int PAGE_SIZE = Configuration.GetValue("PageSize", 10);

      if (!String.IsNullOrEmpty(searchString)) { pageIndex = 1; }
      else { searchString = currentSearch; }

      CurrentSearch = searchString;
      CurrentFilter = currentFilter ?? CurrentFilter;

      var auxiliaryAcademicBodies = !String.IsNullOrEmpty(currentFilter)
        ? GetAllAcademicBodies(currentFilter)
        : GetAllAcademicBodies();

      auxiliaryAcademicBodies = SearchAcademicBody(auxiliaryAcademicBodies, searchString);

      auxiliaryAcademicBodies = String.Equals(sortOrder, "descendant-fullname")
        ? auxiliaryAcademicBodies.OrderByDescending(academicBody => academicBody.Name)
        : auxiliaryAcademicBodies.OrderBy(academicBody => academicBody.Name);

      academicBodies = await PaginatedList<AcademicBodyDto>
        .CreateAsync(auxiliaryAcademicBodies.AsNoTracking(), pageIndex ?? 1, PAGE_SIZE);
    }

    private IQueryable<AcademicBodyDto> SearchAcademicBody(IQueryable<AcademicBodyDto> academicBodies, string search)
    {
      if (!String.IsNullOrEmpty(search))
      {
        academicBodies = academicBodies.Where(academicBody => academicBody.Name.Contains(search));
      }

      return academicBodies;
    }

    private IQueryable<AcademicBodyDto> GetAllAcademicBodies(string filter = "default")
    {
      var academicBodies = _academicBodyService.GetAllAcademicBodies();

      // AssignedStudents = students
      //   .Count(student => student.CourseRegistrations!
      //   .Any(registration => registration.Course.IsOpen));

      // UnassignedStudents = students.Count() - AssignedStudents;

      // if (filter.Equals("assigned"))
      // {
      //   students = students
      //     .Where(student => student.CourseRegistrations!
      //    .Any(registration => registration.Course.IsOpen));
      // }

      // if (filter.Equals("unassigned"))
      // {
      //   students = students
      //     .Where(student => !student.CourseRegistrations!
      //     .Any(registration => registration.Course.IsOpen));
      // }

      return academicBodies;
    }

    // public async Task<FileResult> OnPostExportSpreadsheetData()
    // {
    //   try
    //   {
    //     var students = await GetAllStudents().ToListAsync();
    //     XLWorkbook workbook = new XLWorkbook();
    //     IXLWorksheet worksheet = workbook.Worksheets.Add("Alumnos");

    //     worksheet.Cell(1, 1).Value = "Nombre completo";
    //     worksheet.Cell(1, 2).Value = "Email";
    //     worksheet.Cell(1, 3).Value = "Matrícula";

    //     for (int i = 0; i < students.Count; i++)
    //     {
    //       worksheet.Cell(i + 2, 1).Value = students[i].FullName;
    //       worksheet.Cell(i + 2, 2).Value = students[i].Email;
    //       worksheet.Cell(i + 2, 3).Value = students[i].Enrollment;
    //     }

    //     using (var stream = new MemoryStream())
    //     {
    //       workbook.SaveAs(stream);
    //       var content = stream.ToArray();

    //       return File
    //       (
    //         content,
    //         "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    //         "students.xlsx"
    //       );
    //     }
    //   }
    //   catch (Exception ex)
    //   {
    //     ExceptionLogger.LogException(ex);
    //     TempData["MessageError"] = "Ha ocurrido un error al exportar el archivo";
    //     throw;
    //   }
    // }
  }
}