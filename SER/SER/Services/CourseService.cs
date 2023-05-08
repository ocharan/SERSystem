using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using SER.Models.DB;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;

namespace SER.Services
{
  public class CourseService : ICourseService
  {
    private readonly SERContext _context;
    private readonly IStudentService _studentService;
    private readonly IProfessorService _professorService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private IWebHostEnvironment _environment;


    public CourseService(SERContext context, IMapper mapper, IWebHostEnvironment environment, IFileService fileService, IStudentService studentService, IProfessorService professorService)
    {
      _context = context;
      _studentService = studentService;
      _fileService = fileService;
      _professorService = professorService;
      _mapper = mapper;
      _environment = environment;
    }

    private async Task<Response> CheckRepeatedFields(string nrc)
    {
      Response response = new Response();

      try
      {
        bool isNrcTaken = await _context.Courses
          .AnyAsync(courseFind => courseFind.Nrc.Equals(nrc));

        if (isNrcTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "course.Nrc",
            Message = "El NRC ya está registrado"
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

    private (string, bool) SetCourseDate(string period)
    {
      try
      {
        DateTime currentDate = DateTime.Now;
        string[] selectedDate = period.Split("|");
        DateTime startDate = DateTime.Parse(selectedDate[0].Trim());
        DateTime endDate = DateTime.Parse(selectedDate[1].Trim());
        string courseStartDate = $"{startDate.ToString("MMMM").ToUpper()} {startDate.Year}";
        string courseEndDate = $"{endDate.ToString("MMMM").ToUpper()} {endDate.Year}";
        string courseDate = $"{courseStartDate} - {courseEndDate}";

        bool IsOpen = ((currentDate >= startDate) && (currentDate <= endDate))
          ? true : false;

        return (courseDate, IsOpen);
      }
      catch (FormatException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private async Task<bool> IsCourseExisting(int courseId)
    {
      try
      {
        return await _context.Courses
          .AnyAsync(course => course.CourseId == courseId);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private async Task<bool> AreValidCourseRegistrations(List<CourseRegistrationDto> registrations)
    {
      bool result = true;

      foreach (CourseRegistrationDto registration in registrations)
      {
        bool isStudentExisting = await _studentService.IsStudentExisting(registration.StudentId);

        if (!isStudentExisting)
        {
          result = false;
          break;
        }
      }

      if (result)
      {
        bool isCourseExisting = await IsCourseExisting(registrations[0].CourseId);
        result = isCourseExisting;
      }

      return result;
    }

    private async Task<List<CourseRegistration>> DetermineCourseRegistrationType(List<CourseRegistration> registrations)
    {
      foreach (CourseRegistration registration in registrations)
      {
        bool isFirstRegistration = !(await _context.CourseRegistrations
          .AnyAsync(registrationFind => (registrationFind.StudentId == registration.StudentId) && registrationFind.CourseId == registration.CourseId));

        bool isSecondRegistration = await _context.CourseRegistrations
          .AnyAsync(registrationFind => registrationFind.StudentId == registration.StudentId && registrationFind.CourseId == registration.CourseId && registrationFind.RegistrationType == ECourseRegistrationType.Primera.ToString());

        bool isContinuationRegistration = await _context.CourseRegistrations
          .AnyAsync(registrationFind => registrationFind.StudentId == registration.StudentId && registrationFind.CourseId == registration.CourseId && registrationFind.RegistrationType == ECourseRegistrationType.Segunda.ToString());

        if (isFirstRegistration)
        {
          registration.RegistrationType = ECourseRegistrationType.Primera.ToString();
        }

        if (isSecondRegistration)
        {
          registration.RegistrationType = ECourseRegistrationType.Segunda.ToString();
        }

        if (isContinuationRegistration)
        {
          registration.RegistrationType = ECourseRegistrationType.Continuación.ToString();
        }
      }

      return registrations;
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
      Course courseModel = await _context.Courses
      .Include(course => course.Professor)
        .ThenInclude(professor => professor!.User)
      .Include(course => course.CourseRegistrations)
        .ThenInclude(registration => registration.Student)
      .FirstOrDefaultAsync(course => course.CourseId == courseId)
      ?? throw new ArgumentNullException("Curso no encontrado");

      var course = _mapper.Map<CourseDto>(courseModel);

      if (courseModel.Professor != null)
      {
        course.Professor!.Email = courseModel.Professor.User.Email;
      }

      return course;
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
        var courseValidation = await CheckRepeatedFields(course.Nrc);
        bool isCourseExisting = courseValidation.Errors.Count > 0;
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
          (course.Period, course.IsOpen) = SetCourseDate(course.Period);
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


    public async Task<Response> CreateCourseRegistrations(List<CourseRegistrationDto> registrations)
    {
      Response response = new Response();

      try
      {
        bool areValidRegistrations = await AreValidCourseRegistrations(registrations);

        if (!areValidRegistrations)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "courseRegistrations",
            Message = "Las inscripciones no son válidas"
          });

          return response;
        }

        List<CourseRegistration> courseRegistrations = _mapper.Map<List<CourseRegistration>>(registrations);
        courseRegistrations = await DetermineCourseRegistrationType(courseRegistrations);
        await _context.CourseRegistrations.AddRangeAsync(courseRegistrations);
        await _context.SaveChangesAsync();

        return new Response { IsSuccess = true };
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear las inscripciones");
      }
    }

    public async Task<Response> CreateProfessorAssignment(int courseId, int professorId)
    {
      try
      {
        Response response = new Response();
        bool isProfessorExisting = await _professorService.IsProfessorExisting(professorId);
        bool isCourseExisting = await IsCourseExisting(courseId);

        if (!isProfessorExisting)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "professorId",
            Message = "El profesor no existe"
          });
        }

        if (!isCourseExisting)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "courseId",
            Message = "El curso no existe"
          });
        }

        if (response.Errors.Count == 0)
        {
          Course course = await _context.Courses
            .FirstOrDefaultAsync(course => course.CourseId == courseId)
            ?? throw new ArgumentNullException("Curso no encontrado");

          course.ProfessorId = professorId;
          _context.Courses.Update(course);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al asignar el profesor");
      }

    }
  }
}