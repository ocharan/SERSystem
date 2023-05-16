using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;

namespace SER.Pages.Course
{
  public class UpdateModel : PageModel
  {
    private readonly ICourseService _courseService;
    public Response response { get; set; } = null!;
    [BindProperty]
    public CourseDto course { get; set; } = null!;
    [BindProperty]
    public IFormFile? fileUpload { get; set; }
    public string? FilePath { get; set; }

    public UpdateModel(ICourseService courseService) { _courseService = courseService; }

    public async Task<IActionResult> OnGet(string courseId)
    {
      try
      {
        int.TryParse(courseId, out int id);
        course = await _courseService.GetCourse(id);

        if (course.FileId != null)
        {
          FilePath = (await _courseService.GetCourseFile(course.FileId.Value)).Path
            .Split("/")
            .Last();
        }
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);

        return RedirectToPage("/Course/Index");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostUpdateCourse()
    {
      if (!ModelState.IsValid) { return Page(); }

      try
      {
        if (fileUpload != null)
        {
          response = await _courseService.UpdateCourse(course, fileUpload);
        }
        else
        {
          response = await _courseService.UpdateCourse(course);
        }

        if (response.Errors.Count > 0 && !response.IsSuccess)
        {
          foreach (var error in response.Errors)
          {
            ModelState.AddModelError(error.FieldName, error.Message);
          }

          return Page();
        }

        TempData["MessageSuccess"] = EStatusCodes.Ok;
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/Course/Index");
    }

    public async Task<IActionResult> OnPostDeleteCourseFile()
    {
      try
      {
        response = await _courseService.DeleteCourseFile(course.FileId!.Value);

        if (response.Errors.Count > 0 && !response.IsSuccess)
        {
          foreach (var error in response.Errors)
          {
            ModelState.AddModelError(error.FieldName, error.Message);
          }
        }
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/Course/Update", new { courseId = course.CourseId });
    }
  }
}