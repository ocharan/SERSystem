using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.DB;
using SER.Models.DTO;

namespace SER.Services
{
  public class ProfessorService : IProfessorService
  {
    private readonly SERContext _context;
    private readonly IMapper _mapper;

    public ProfessorService(SERContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    private IQueryable<ProfessorDto> GetAssignedProfessors()
    {
      try
      {
        var professors = _context.Professors
          .Where(course => _context.Courses
          .Any(course => course.IsOpen && course.ProfessorId == course.ProfessorId));

        return professors.ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private IQueryable<ProfessorDto> GetUnassignedProfessors()
    {
      try
      {
        var professors = _context.Professors
          .Where(professor => !_context.Courses
          .Any(course => course.IsOpen && course.ProfessorId == professor.ProfessorId));

        return professors.ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    // private Dictionary<string, bool> IsProfessorExisting(ProfessorDto professorDto)
    // {
    //   try
    //   {
    //     Dictionary<string, bool> isTaken = new Dictionary<string, bool>();

    //     isTaken.Add("IsIdTaken", _context.Professors
    //       .Any(professor => professor.Id == professorDto.Id));

    //     return isTaken;
    //   }
    //   catch (ArgumentNullException ex)
    //   {
    //     ExceptionLogger.LogException(ex);
    //     throw;
    //   }
    // } 

    public (IQueryable<ProfessorDto> Professors, int AssignedCount, int UnassignedCount) GetAllProfessors(string filter)
    {
      IQueryable<ProfessorDto> professors = new List<ProfessorDto>().AsQueryable();

      try
      {
        if (filter == "assigned") { professors = GetAssignedProfessors(); }
        else if (filter == "unassigned") { professors = GetUnassignedProfessors(); }
        else { professors = GetAssignedProfessors().Concat(GetUnassignedProfessors()); }

        return (
          professors,
          GetAssignedProfessors().Count(),
          GetUnassignedProfessors().Count()
        );
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }
  }
}