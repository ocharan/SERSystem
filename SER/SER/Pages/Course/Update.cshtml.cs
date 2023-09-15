using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;
using SER.Models.DTO;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace SER.Pages.Course
{
  [Authorize(Roles = (nameof(ERoles.Administrator)) + "," + (nameof(ERoles.Professor)))]
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

        // course.Period = await FormatCoursePeriod(course.Period);
        ViewData["MessageSuccess"] = TempData["MessageSuccess"];
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
      ModelState.Remove("course.Period");

      if (!ModelState.IsValid) { return Page(); }

      try
      {
        response = await _courseService.UpdateCourse(course, fileUpload);

        if (!response.IsSuccess)
        {
          HandleModelErrors(response.Errors);

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

        if (response.IsSuccess) { TempData["MessageSuccess"] = EStatusCodes.Ok; }
        else { HandleModelErrors(response.Errors); }
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is OperationCanceledException)
      {
        ExceptionLogger.LogException(ex);
        TempData["MessageError"] = ex.Message;
      }

      return RedirectToPage("/Course/Update", new { courseId = course.CourseId });
    }

    private Task<string> FormatCoursePeriod(string period)
    {
      string[] dates = period.Split("_");
      DateTime startDate = DateTime.ParseExact(dates[0], "MMMMyyyy", null);
      DateTime endDate = DateTime.ParseExact(dates[1], "MMMMyyyy", null);

      string formattedPeriod = $"{startDate.ToString("MMMM yyyy")} - {endDate.ToString("MMMM yyyy")}";

      return Task.FromResult(formattedPeriod);
    }

    private void HandleModelErrors(List<FieldError> errors)
    {
      foreach (var error in errors)
      {
        ModelState.AddModelError(error.FieldName, error.Message);
      }
    }
  }
}