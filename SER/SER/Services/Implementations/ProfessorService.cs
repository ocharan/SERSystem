using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.DB;
using SER.Models.DTO;
using Microsoft.EntityFrameworkCore;

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

    public IQueryable<ProfessorDto> GetAllProfessors()
    {
      IQueryable<ProfessorDto> professors = new List<ProfessorDto>().AsQueryable();

      try
      {
        return _context.Professors
          .ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<List<ProfessorDto>> SearchProfessor(string search)
    {
      try
      {
        var professors = await _context.Professors
          .Where(professor => professor.FullName.Contains(search) || _context.Users
            .Any(user => user.UserId == professor.UserId && user.Email.Contains(search)))
          .ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider)
          .ToListAsync();

        professors
          .ForEach(professor => professor.Email = _context.Users
            .Where(user => user.UserId == professor.UserId)
            .Select(user => user.Email)
            .FirstOrDefault()!);

        return professors;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<bool> IsProfessorExisting(int professorId)
    {
      try
      {
        return await _context.Professors
          .AnyAsync(professor => professor.ProfessorId == professorId);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<ProfessorDto> GetProfessor(int professorId)
    {
      var professor = await _context.Professors
        .FirstOrDefaultAsync(professor => professor.ProfessorId == professorId)
        ?? throw new ArgumentNullException("Profesor no encontrado");

      return _mapper.Map<ProfessorDto>(professor);

    }
  }
}