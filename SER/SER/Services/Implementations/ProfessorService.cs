using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.DB;
using SER.Models.DTO;
using Microsoft.EntityFrameworkCore;
using SER.Models.Responses;
using SER.Models.Enums;
using SER.Services.Contracts;

namespace SER.Services
{
  public class ProfessorService : IProfessorService
  {
    private readonly SERContext _context;
    private readonly IUserService _userService;
    // private readonly IAcademicBodyService _academicBodyService;
    private readonly IMapper _mapper;

    public ProfessorService(SERContext context, IMapper mapper, IUserService userService)
    {
      _context = context;
      _mapper = mapper;
      _userService = userService;
      // _academicBodyService = academicBodyService;
    }

    private async Task<Response> CheckRepeatedFields(ProfessorDto professorDto)
    {
      try
      {
        Response response = new Response();

        bool isEmailTaken = await _context.Users
          .AnyAsync(userFind => userFind.Email
          .Equals(professorDto.Email.Replace(" ", "")));

        if (isEmailTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "professor.Email",
            Message = "El correo electrónico ya está registrado."
          });
        }

        return response;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public IQueryable<ProfessorDto> GetAllProfessors()
    {
      try
      {
        var professors = _context.Professors
          .Include(professor => professor.Courses)
          .AsSplitQuery()
          .AsQueryable()
          .ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider);

        return professors;
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
          .AsSplitQuery()
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

    public async Task<List<ProfessorDto>> SearchMember(string search)
    {
      try
      {
        var professors = await _context.Professors
          .Where(professor => professor.FullName.Contains(search) || _context.Users
            .Any(user => user.UserId == professor.UserId && user.Email.Contains(search)))
          .AsSplitQuery()
          .ProjectTo<ProfessorDto>(_mapper.ConfigurationProvider)
          .ToListAsync();

        var members = _context.AcademicBodyMembers
          .Select(professor => professor.ProfessorId)
          .ToList();

        professors = professors.Where(professor => !members.Contains(professor.ProfessorId))
          .ToList();

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
      Professor professor = await _context.Professors
        .Include(professor => professor.Courses)
          .ThenInclude(course => course.CourseRegistrations)
        .AsSplitQuery()
        .FirstOrDefaultAsync(professor => professor.ProfessorId == professorId)
        ?? throw new NullReferenceException("Profesor no encontrado");

      ProfessorDto professorDto = _mapper.Map<ProfessorDto>(professor);
      professorDto.Email = await _userService.GetUserEmailById(professor.UserId);

      return professorDto;
    }

    public async Task<Response> CreateProfessor(ProfessorDto professorDto)
    {
      try
      {
        string email = professorDto.Email;
        string username = email.Split('@')[0];
        var professor = _mapper.Map<Professor>(professorDto);
        professor.User = new User
        {
          Email = email,
          Username = username,
          Password = BCrypt.Net.BCrypt.HashPassword(username),
          Role = ERoles.Professor.ToString()
        };

        await _context.Professors.AddAsync(professor);
        await _context.SaveChangesAsync();

        return new Response { IsSuccess = true };
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<Response> UpdateProfessor(ProfessorDto professorDto)
    {
      try
      {
        Response response = await CheckRepeatedFields(professorDto);
        string email = (await GetProfessor(professorDto.ProfessorId)).Email;
        bool isCurrentEmail = email.Equals(professorDto.Email);
        bool isEmailTaken = isCurrentEmail
          ? false
          : response.Errors.Any(error => error.FieldName.Equals("professor.Email"));

        if (!isEmailTaken)
        {
          Professor professor = await _context.Professors
            .FindAsync(professorDto.ProfessorId)
            ?? throw new NullReferenceException("Profesor no encontrado");

          professor.FullName = professorDto.FullName;
          professor.AcademicDegree = professorDto.AcademicDegree;
          professor.StudyField = professorDto.StudyField;
          professor.User.Email = professorDto.Email;
          _context.Professors.Update(professor);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
          response.Errors.Clear();
        }

        return response;
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException(ex.Message);
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al actualizar el profesor.");
      }
    }
  }
}