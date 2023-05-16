using SER.Models.DTO;

namespace SER.Services
{
  public interface IProfessorService
  {
    IQueryable<ProfessorDto> GetAllProfessors();
    Task<List<ProfessorDto>> SearchProfessor(string search);
    Task<bool> IsProfessorExisting(int professorId);
    Task<ProfessorDto> GetProfessor(int professorId);
  }
}