using SER.Models.DTO;

namespace SER.Services
{
  public interface IProfessorService
  {
    (IQueryable<ProfessorDto> Professors, int AssignedCount, int UnassignedCount) GetAllProfessors(string filter);
  }
}