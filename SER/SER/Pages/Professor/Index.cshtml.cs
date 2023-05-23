using ClosedXML.Excel;
using ContosoUniversity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SER.Configuration;
using SER.Models.DTO;
using SER.Models.Enums;
using SER.Services;

namespace SER.Pages.Professor
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class IndexModel : PageModel
  {
    private readonly IConfiguration Configuration;
    private readonly IProfessorService _professorService;
    private readonly IUserService _userService;
    public string? FullnameSort { get; set; }
    public string? CurrentSort { get; set; }
    public string? CurrentSearch { get; set; }
    public string? CurrentFilter { get; set; }
    public int AssignedProfessors { get; set; }
    public int UnassignedProfessors { get; set; }
    public PaginatedList<ProfessorDto> professors { get; set; } = null!;

    public IndexModel(IProfessorService professorService, IConfiguration configuration, IUserService userService)
    {
      _professorService = professorService;
      Configuration = configuration;
      _userService = userService;
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

      var auxiliaryProfessors = !String.IsNullOrEmpty(currentFilter)
        ? GetAllProfessors(currentFilter)
        : GetAllProfessors();

      auxiliaryProfessors = SearchProfessor(auxiliaryProfessors, searchString);

      auxiliaryProfessors = String.Equals(sortOrder, "descendant-fullname")
        ? auxiliaryProfessors.OrderByDescending(professor => professor.FullName)
        : auxiliaryProfessors.OrderBy(professor => professor.FullName);

      professors = await PaginatedList<ProfessorDto>.CreateAsync(
        auxiliaryProfessors.AsNoTracking(), pageIndex ?? 1, PAGE_SIZE);

      foreach (var professor in professors)
      {
        professor.Email = await _userService.GetUserEmailById(professor.UserId);
      }
    }

    private IQueryable<ProfessorDto> GetAllProfessors(string filter = "default")
    {
      var professors = _professorService.GetAllProfessors();
      AssignedProfessors = professors.Count(professor => professor.Courses!.Count() > 0);
      UnassignedProfessors = professors.Count() - AssignedProfessors;

      if (filter.Equals("assigned"))
      {
        professors = professors.Where(professor => professor.Courses!.Count() > 0);
      }

      if (filter.Equals("unassigned"))
      {
        professors = professors.Where(professor => professor.Courses!.Count() == 0);
      }

      return professors;
    }

    private IQueryable<ProfessorDto> SearchProfessor(IQueryable<ProfessorDto> professors, string search)
    {
      if (!String.IsNullOrEmpty(search))
      {
        professors = professors.Where(professor => professor.FullName!.Contains(search));
      }

      return professors;
    }

    public async Task<FileResult> OnPostExportSpreadsheetData()
    {
      try
      {
        var professors = await GetAllProfessors().ToListAsync();
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Alumnos");

        worksheet.Cell(1, 1).Value = "Nombre completo";
        worksheet.Cell(1, 2).Value = "Email";
        worksheet.Cell(1, 3).Value = "Grado académico";

        for (int i = 0; i < professors.Count; i++)
        {
          worksheet.Cell(i + 2, 1).Value = professors[i].FullName;
          worksheet.Cell(i + 2, 2).Value = await _userService
            .GetUserEmailById(professors[i].UserId);
          worksheet.Cell(i + 2, 3).Value = professors[i].AcademicDegree;
        }

        using (var stream = new MemoryStream())
        {
          workbook.SaveAs(stream);
          var content = stream.ToArray();

          return File
          (
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "profesores.xlsx"
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
  }
}