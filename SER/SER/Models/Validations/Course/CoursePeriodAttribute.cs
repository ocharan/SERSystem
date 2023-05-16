using System.ComponentModel.DataAnnotations;
using SER.Models.Enums;

namespace SER.Models.Validations.Course
{
  public class CoursePeriodAttribute : ValidationAttribute
  {
    public override bool IsValid(object? value)
    {
      string? period = value as string;

      if (!string.IsNullOrEmpty(period))
      {
        if (!Enum.IsDefined(typeof(EPeriods), period))
        {
          return false;
        }
      }

      return true;
    }
  }
}