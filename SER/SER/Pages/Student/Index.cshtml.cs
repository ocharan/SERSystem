using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SER.Configuration;

namespace SER.Pages.Student
{
  [Authorize(Roles = "Administrador")]
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

    public async Task OnGet(string sortOrder, string currentSearch, string searchString, int? pageIndex, string currentFilter)
    {
      if (TempData["MessageSuccess"] != null) { ViewData["MessageSuccess"] = TempData["MessageSuccess"]; }

      if (TempData["MessageError"] != null) { ViewData["MessageError"] = TempData["MessageError"]; }

      CurrentSort = sortOrder;
      FullnameSort = String.IsNullOrEmpty(sortOrder) ? "descendant-fullname" : "";
      int PAGE_SIZE = Configuration.GetValue("PageSize", 10);

      if (!String.IsNullOrEmpty(searchString)) { pageIndex = 1; }
      else { searchString = currentSearch; }

      CurrentSearch = searchString;
      // CurrentSort = sortOrder ?? CurrentSort;
      CurrentFilter = currentFilter ?? CurrentFilter;

      var auxiliaryStudents = !String.IsNullOrEmpty(currentFilter)
        ? GetAllStudents(currentFilter)
        : GetAllStudents();

      if (!String.IsNullOrEmpty(searchString))
      {
        auxiliaryStudents = auxiliaryStudents.Where(student =>
          student.FullName!.Contains(searchString)
        );
      }

      auxiliaryStudents = String.Equals(sortOrder, "descendant-fullname")
        ? auxiliaryStudents.OrderByDescending(student => student.FullName)
        : auxiliaryStudents.OrderBy(student => student.FullName);

      students = await PaginatedList<StudentDto>.CreateAsync(
        auxiliaryStudents.AsNoTracking(), pageIndex ?? 1, PAGE_SIZE
      );
    }

    public IQueryable<StudentDto> GetAllStudents(string filter = "default")
    {
      IQueryable<StudentDto> students = new List<StudentDto>().AsQueryable();

      try
      {
        students = _studentService.GetAllStudents();
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }

      AssignedStudents = students
        .Count(student => student.CourseRegistrations!
        .Any(registration => registration.Course.IsOpen));

      UnassignedStudents = students
        .Count(student => !student.CourseRegistrations!
        .Any(registration => registration.Course.IsOpen));

      if (filter.Equals("assigned"))
      {
        students = students
          .Where(student => student.CourseRegistrations!
          .Any(registration => registration.Course.IsOpen))
          .AsQueryable();
      }

      if (filter.Equals("unassigned"))
      {
        students = students
          .Where(student => !student.CourseRegistrations!
          .Any(registration => registration.Course.IsOpen))
          .AsQueryable();
      }

      return students;
    }

    public async Task<FileResult> OnPostExportSpreadsheetData()
    {
      try
      {
        var students = await _studentService.GetAllStudents().ToListAsync();
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Students");
        worksheet.Cell(1, 1).Value = "Full Name";
        worksheet.Cell(1, 2).Value = "Email";
        worksheet.Cell(1, 3).Value = "Enrollment";

        for (int i = 0; i < students.Count; i++)
        {
          worksheet.Cell(i + 2, 1).Value = students[i].FullName;
          worksheet.Cell(i + 2, 2).Value = students[i].Email;
          worksheet.Cell(i + 2, 3).Value = students[i].Enrollment;
        }

        MemoryStream memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        byte[] content = memoryStream.ToArray();

        return File(
          content,
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "students.xlsx"
        );
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = "Ha ocurrido un error al exportar el archivo";
        throw;
      }
    }
  }
}