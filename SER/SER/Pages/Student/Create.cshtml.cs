using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Services;

namespace SER.Pages.Student
{
  [Authorize(Roles = "Administrador")]
  public class CreateModel : PageModel
  {
    private readonly IStudentService _studentService;
    [BindProperty]
    public StudentDto student { get; set; } = null!;

    public CreateModel(IStudentService studentService) { _studentService = studentService; }

    public IActionResult OnGet() { return Page(); }

    public async Task<IActionResult> OnPostCreateStudent()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        Dictionary<string, bool> result = await _studentService.CreateStudent(student);

        if (result.ContainsKey("IsCreated") && result["IsCreated"])
        {
          TempData["MessageSuccess"] = "Se ha verificado la información del alumno y se ha registrado correctamente.";
        }
        else
        {
          if (result["IsEnrollmentTaken"])
          {
            ModelState.AddModelError("student.Enrollment", "La matrícula ya está registrada.");
          }

          if (result["IsEmailTaken"])
          {
            ModelState.AddModelError("student.Email", "El correo electrónico ya está registrado.");
          }

          return Page();
        }
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