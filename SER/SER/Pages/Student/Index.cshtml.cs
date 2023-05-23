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

namespace SER.Pages.Student
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class IndexModel : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly IStudentService _studentService;
    public string? FullnameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    public int AssignedStudents { get; set; }
    public int UnassignedStudents { get; set; }
    public PaginatedList<StudentDto> students { get; set; } = null!;

    public IndexModel(IStudentService studentService, IConfiguration configuration)
    {
      _studentService = studentService;
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

      var auxiliaryStudents = !String.IsNullOrEmpty(currentFilter)
        ? GetAllStudents(currentFilter)
        : GetAllStudents();

      auxiliaryStudents = SearchStudent(auxiliaryStudents, searchString);

      auxiliaryStudents = String.Equals(sortOrder, "descendant-fullname")
        ? auxiliaryStudents.OrderByDescending(student => student.FullName)
        : auxiliaryStudents.OrderBy(student => student.FullName);

      students = await PaginatedList<StudentDto>
        .CreateAsync(auxiliaryStudents.AsNoTracking(), pageIndex ?? 1, PAGE_SIZE);
    }

    private IQueryable<StudentDto> GetAllStudents(string filter = "default")
    {
      var students = _studentService.GetAllStudents();

      AssignedStudents = students
        .Count(student => student.CourseRegistrations!
        .Any(registration => registration.Course.IsOpen));

      UnassignedStudents = students.Count() - AssignedStudents;

      if (filter.Equals("assigned"))
      {
        students = students
          .Where(student => student.CourseRegistrations!
         .Any(registration => registration.Course.IsOpen));
      }

      if (filter.Equals("unassigned"))
      {
        students = students
          .Where(student => !student.CourseRegistrations!
          .Any(registration => registration.Course.IsOpen));
      }

      return students;
    }

    public async Task<FileResult> OnPostExportSpreadsheetData()
    {
      try
      {
        var students = await GetAllStudents().ToListAsync();
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Alumnos");

        worksheet.Cell(1, 1).Value = "Nombre completo";
        worksheet.Cell(1, 2).Value = "Email";
        worksheet.Cell(1, 3).Value = "Matrícula";

        for (int i = 0; i < students.Count; i++)
        {
          worksheet.Cell(i + 2, 1).Value = students[i].FullName;
          worksheet.Cell(i + 2, 2).Value = students[i].Email;
          worksheet.Cell(i + 2, 3).Value = students[i].Enrollment;
        }

        using (var stream = new MemoryStream())
        {
          workbook.SaveAs(stream);
          var content = stream.ToArray();

          return File
          (
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "students.xlsx"
          );
        }
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = "Ha ocurrido un error al exportar el archivo";
        throw;
      }
    }

    private IQueryable<StudentDto> SearchStudent(IQueryable<StudentDto> students, string search)
    {
      if (!String.IsNullOrEmpty(search))
      {
        students = students.Where(student => student.FullName!.Contains(search));
      }

      return students;
    }
  }
}