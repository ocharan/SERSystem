using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface IProfessorService
  {
    IQueryable<ProfessorDto> GetAllProfessors();
    Task<List<ProfessorDto>> SearchProfessor(string search);
    Task<List<ProfessorDto>> SearchMember(string search);
    Task<bool> IsProfessorExisting(int professorId);
    Task<ProfessorDto> GetProfessor(int professorId);
    Task<Response> CreateProfessor(ProfessorDto professorDto);
    Task<Response> UpdateProfessor(ProfessorDto professorDto);
  }
}