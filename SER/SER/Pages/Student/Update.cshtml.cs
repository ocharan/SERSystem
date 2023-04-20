using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace SER.Pages.Student
{
  [Authorize(Roles = "Administrador")]
  public class UpdateModel : PageModel
  {
    private readonly IStudentService _studentService;
    [BindProperty]
    public StudentDto student { get; set; } = null!;

    public UpdateModel(IStudentService studentService) { _studentService = studentService; }

    public async Task<IActionResult> OnGet(string studentId)
    {
      try
      {
        int.TryParse(studentId, out int id);
        student = await _studentService.GetStudent(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Student/Index");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostUpdateStudent()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        Dictionary<string, bool> result = await _studentService.UpdateStudent(student);

        if (result.ContainsKey("IsUpdated") && result["IsUpdated"])
        {
          TempData["MessageSuccess"] = "Se ha verificado la información del alumno y se ha actualizado correctamente.";
        }

        if (result["IsEmailTaken"])
        {
          ModelState.AddModelError("student.Email", "El correo electrónico ya está registrado.");

          return Page();
        }
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/Student/Index");
    }
  }
}