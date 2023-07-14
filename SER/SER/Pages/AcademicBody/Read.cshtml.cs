using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Configuration;
using SER.Models.DTO;
using SER.Services;
using SER.Models.Enums;
using System.Net;
using SER.Services.Contracts;

namespace SER.Pages.AcademicBody
{
  [Authorize(Roles = nameof(ERoles.Administrator))]
  public class ReadModel : PageModel
  {
    private readonly IAcademicBodyService _academicBodyService;
    [BindProperty]
    public AcademicBodyDto academicBody { get; set; } = null!;
    [BindProperty]
    public LgacDto lgac { get; set; } = null!;

    public ReadModel(IAcademicBodyService academicBodyService) { _academicBodyService = academicBodyService; }

    public async Task<IActionResult> OnGet(string academicBodyId)
    {
      try
      {
        int.TryParse(academicBodyId, out int id);
        academicBody = await _academicBodyService.GetAcademicBody(id);
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/AcademicBody/Index");
      }

      return Page();
    }

    public async Task<JsonResult> OnPostCreateLgac([FromBody] LgacDto lgacDto)
    {
      try
      {
        await _academicBodyService.CreateLgac(lgacDto);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
    }

    public async Task<JsonResult> OnPostUpdateLgac([FromBody] LgacDto lgacDto)
    {
      try
      {
        await _academicBodyService.UpdateLgac(lgacDto);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
    }

    public async Task<JsonResult> OnPostDeleteLgac([FromBody] LgacDto lgacDto)
    {
      try
      {
        System.Console.WriteLine($"lgacId: {lgacDto.LgacId}");

        await _academicBodyService.DeleteLgac(lgacDto.LgacId);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
    }

    public async Task<JsonResult> OnPostCreateMembers([FromBody] List<AcademicBodyMemberDto> members)
    {
      try
      {
        await _academicBodyService.CreateAcademicBodyMembers(members);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
    }

    public async Task<JsonResult> OnPostDeleteMember([FromBody] int memberId)
    {
      try
      {
        await _academicBodyService.DeleteMember(memberId);

        return new JsonResult("Success Message")
        {
          StatusCode = StatusCodes.Status200OK
        };
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);

        return new JsonResult("Error Message")
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
    }
  }
}