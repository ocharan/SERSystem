using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SER.Models.DB;
using SER.Models.DTO;
using SER.Configuration;
using AutoMapper.QueryableExtensions;

namespace SER.Services
{
  public class CourseService : ICourseService
  {
    private readonly SERContext _context;
    private readonly IMapper _mapper;

    public CourseService(SERContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    private IQueryable<CourseDto> GetOpenCourses()
    {
      try
      {
        var courses = _context.Courses.Where(course => course.IsOpen);

        return courses.ProjectTo<CourseDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private IQueryable<CourseDto> GetClosedCourses()
    {
      try
      {
        var courses = _context.Courses.Where(course => !course.IsOpen);

        return courses.ProjectTo<CourseDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private Dictionary<string, bool> IsCourseExisting(CourseDto courseDto)
    {
      Dictionary<string, bool> isTaken = new();

      try
      {
        isTaken.Add("IsNrcTaken", _context.Courses
          .Any(course => course.Nrc == courseDto.Nrc));

        return isTaken;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public (IQueryable<CourseDto> Courses, int OpenCount, int ClosedCount) GetAllCourses(string filter)
    {
      IQueryable<CourseDto> courses = new List<CourseDto>().AsQueryable();

      try
      {
        switch (filter)
        {
          case "open":
            courses = GetOpenCourses();

            return (
              courses,
              GetOpenCourses().Count(),
              GetClosedCourses().Count()
            );

          case "closed":
            courses = GetClosedCourses();

            return (
              courses,
              GetOpenCourses().Count(),
              GetClosedCourses().Count()
            );

          default:
            courses = GetOpenCourses();
            courses.Concat(GetClosedCourses());

            return (
              courses,
              GetOpenCourses().Count(),
              GetClosedCourses().Count()
            );
        }
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<CourseDto> GetCourse(int courseId)
    {
      Course course = await _context.Courses
        .Include(course => course.Professor)
        .Include(course => course.CourseRegistrations)
        .ThenInclude(registration => registration.Student)
        .FirstOrDefaultAsync(course => course.CourseId == courseId)
        ?? throw new ArgumentNullException("Curso no encontrado");

      return _mapper.Map<CourseDto>(course);
    }

    public async Task<List<CourseRegistrationDto>> GetStudentCourses(int studentId)
    {
      try
      {
        List<CourseRegistration> registrations = await _context.CourseRegistrations
          .Include(registration => registration.Course)
          .ThenInclude(course => course.Professor)
          .Where(registration => registration.StudentId == studentId)
          .ToListAsync();

        return _mapper.Map<List<CourseRegistrationDto>>(registrations);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<Dictionary<string, bool>> CreateCourse(CourseDto courseDto)
    {
      try
      {
        Course course = _mapper.Map<Course>(courseDto);
        Dictionary<string, bool> isTaken = IsCourseExisting(courseDto);

        if (!isTaken["IsNrcTaken"])
        {
          await _context.Courses.AddAsync(course);
          await _context.SaveChangesAsync();
          isTaken.Add("IsCreated", true);
        }

        return isTaken;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear el curso");
      }
    }

    public async Task<Dictionary<string, bool>> UpdateCourse(CourseDto courseDto)
    {
      try
      {
        Dictionary<string, bool> isTaken = IsCourseExisting(courseDto);
        int nrc = (await GetCourse(courseDto.CourseId)).Nrc;
        bool isCurrentNrc = nrc == courseDto.Nrc;
        bool isNrcTaken = isCurrentNrc ? false : isTaken["IsNrcTaken"];

        if (!isNrcTaken)
        {
          Course course = await _context.Courses
            .FindAsync(courseDto.CourseId)
            ?? throw new ArgumentNullException("Curso no encontrado");

          course.Name = courseDto.Name;
          course.Nrc = courseDto.Nrc;
          course.ProfessorId = courseDto.ProfessorId;
          course.IsOpen = courseDto.IsOpen;
          _context.Courses.Update(course);
          await _context.SaveChangesAsync();
          isTaken.Add("IsUpdated", true);
          isTaken["IsNrcTaken"] = false;
        }

        return isTaken;
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException(ex.Message);
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al actualizar el curso");
      }
    }
  }
}