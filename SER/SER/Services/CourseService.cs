using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using SER.Models.DB;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.Responses;

namespace SER.Services
{
  public class CourseService : ICourseService
  {
    private readonly SERContext _context;
    private readonly IMapper _mapper;
    private IWebHostEnvironment _environment;

    private readonly IFileService _fileService;

    public CourseService(SERContext context, IMapper mapper, IWebHostEnvironment environment, IFileService fileService)
    {
      _context = context;
      _mapper = mapper;
      _environment = environment;
      _fileService = fileService;
    }

    public async Task<bool> IsCourseExisting(string nrc)
    {
      try
      {
        return await _context.Courses
          .AnyAsync(course => course.Nrc.Equals(nrc));
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public IQueryable<CourseDto> GetAllCourses()
    {
      try
      {
        var courses = _context.Courses
          .Include(course => course.Professor)
          .Include(course => course.CourseRegistrations)
          .ThenInclude(registration => registration.Student)
          .ProjectTo<CourseDto>(_mapper.ConfigurationProvider);

        return courses;
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

    public async Task<Response> CreateCourse(CourseDto courseDto, IFormFile? file = null)
    {
      try
      {
        var course = _mapper.Map<Course>(courseDto);
        bool isCourseExisting = await IsCourseExisting(course.Nrc);
        Response response = new Response();

        if (file != null && file.Length > 0)
        {
          response = await _fileService.ValidateFile(file);

          if (response.IsSuccess)
          {
            string path = await _fileService.SaveFile(file!, courseDto.Nrc);
            course.File = new CourseFile { Path = path };
          }

          response.IsSuccess = false;
        }

        if (isCourseExisting)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "course.Nrc",
            Message = "El NRC ya existe"
          });

          return response;
        }

        if (response.Errors.Count == 0)
        {
          await _context.Courses.AddAsync(course);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear el curso");
      }
    }
  }
}