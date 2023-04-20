using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SER.Services;

namespace SER.Pages.Course
{
  public class CreateModel : PageModel
  {
    private readonly ICourseService _courseService;
    public CreateModel(ICourseService courseService)
    {
      _courseService = courseService;
    }

    public void OnGet()
    {
    }
  }
}