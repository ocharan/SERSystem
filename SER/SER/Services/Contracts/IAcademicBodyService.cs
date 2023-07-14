using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services.Contracts
{
  public interface IAcademicBodyService
  {
    IQueryable<AcademicBodyDto> GetAllAcademicBodies();
    Task<AcademicBodyDto> GetAcademicBody(int academicBodyId);
    Task<Response> CreateAcademicBody(AcademicBodyDto academicBody);
    Task<Response> UpdateAcademicBody(AcademicBodyDto academicBody);
    Task<Response> CreateLgac(LgacDto lgac);
    Task<Response> UpdateLgac(LgacDto lgac);
    Task<Response> DeleteLgac(int lgacId);
    Task<Response> CreateAcademicBodyMembers(List<AcademicBodyMemberDto> members);
    Task<Response> DeleteMember(int memberId);
  }
}