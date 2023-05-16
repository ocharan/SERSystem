using System.ComponentModel.DataAnnotations;

namespace SER.Models.DTO
{
  public class CourseFileDto
  {
    public int FileId { get; set; }
    public string Path { get; set; } = null!;

    public ICollection<CourseDto> Courses { get; set; } = null!;
  }
}