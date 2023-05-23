using System.ComponentModel.DataAnnotations;
using SER.Models.Enums;

namespace SER.Models.Validations.Professor
{
  public class ProfessorAcademicDegreeAttribute : ValidationAttribute
  {
    public override bool IsValid(object? value)
    {
      string? academicDegree = value as string;

      if (!string.IsNullOrEmpty(academicDegree))
      {
        if (!Enum.IsDefined(typeof(EAcademicDegree), academicDegree))
        {
          return false;
        }
      }

      return true;
    }
  }
}