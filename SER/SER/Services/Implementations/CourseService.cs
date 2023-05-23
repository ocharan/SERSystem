using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using SER.Models.DB;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;
using System.Globalization;

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
    Response response = new Response();

    public CourseService(SERContext context, IMapper mapper, IWebHostEnvironment environment, IProfessorService professorService,
      IFileService fileService, IStudentService studentService)
    {
      _context = context;
      _studentService = studentService;
      _fileService = fileService;
      _professorService = professorService;
      _mapper = mapper;
      _environment = environment;
    }

    private async Task<Response> CheckRepeatedFields(CourseDto course)
    {
      try
      {
        bool isNrcTaken = await _context.Courses
          .AnyAsync(courseFind => courseFind.Nrc
          .Equals(course.Nrc.Replace(" ", "")));

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

    private bool IsCourseOpen(string period)
    {
      try
      {
        DateTime currentDate = DateTime.Now;
        string[] dates = period.Split("_");

        DateTime startDate = DateTime
          .ParseExact(dates[0], "MMMMyyyy", CultureInfo
          .CreateSpecificCulture("es-MX"));

        DateTime endDate = DateTime
          .ParseExact(dates[1], "MMMMyyyy", CultureInfo
          .CreateSpecificCulture("es-MX"));

        bool IsOpen = ((currentDate >= startDate) && (currentDate <= endDate))
          ? true : false;

        return IsOpen;
      }
      catch (FormatException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private async Task<List<CourseRegistration>> DetermineCourseRegistrationType(List<CourseRegistration> registrations)
    {
      string courseName = (await GetCourse(registrations[0].CourseId)).Name.Replace(" ", string.Empty);

      foreach (var registration in registrations)
      {
        if (courseName.Equals(ECourseNames.ProyectoGuiado.ToString()))
        {
          int count = await _context.CourseRegistrations
            .CountAsync(courseRegistration => courseRegistration.StudentId == registration.StudentId
              && !courseRegistration.RegistrationType.Equals(ERegistrationType.Baja.ToString())
              && courseRegistration.Course.Name
                .Replace(" ", string.Empty)
                .Equals(ECourseNames.ProyectoGuiado
                .ToString()));

          if (count == 0) { registration.RegistrationType = ERegistrationType.Primera.ToString(); }
          else if (count == 1) { registration.RegistrationType = ERegistrationType.Segunda.ToString(); }
          else { new ArgumentNullException("El alumno ya alcanzo el número máximo de inscripciones"); }
        }

        if (courseName.Equals(ECourseNames.ExperienciaRecepcional.ToString()))
        {
          int count = await _context.CourseRegistrations
            .CountAsync(courseRegistration => courseRegistration.StudentId == registration.StudentId
              && !courseRegistration.RegistrationType.Equals(ERegistrationType.Baja.ToString())
              && courseRegistration.Course.Name
                .Replace(" ", string.Empty)
                .Equals(ECourseNames.ExperienciaRecepcional
                .ToString()));

          if (count == 0) { registration.RegistrationType = ERegistrationType.Primera.ToString(); }
          else if (count == 1) { registration.RegistrationType = ERegistrationType.Segunda.ToString(); }
          else if (count == 2) { registration.RegistrationType = ERegistrationType.Continuación.ToString(); }
          else { new ArgumentNullException("El alumno ya alcanzo el número máximo de inscripciones"); }
        }
      }

      return registrations;
    }

    private async Task<bool> IsValidCourseUnregistration(int registrationId)
    {
      try
      {
        CourseRegistration registration = await _context.CourseRegistrations
          .Include(registrationFind => registrationFind.Course)
          .FirstOrDefaultAsync(registrationFind => registrationFind.CourseRegistrationId == registrationId)
          ?? throw new NullReferenceException("La inscripción no existe");

        int registrationCount = await _context.CourseRegistrations
          .CountAsync(registrationFind =>
            registrationFind.Course.Name.Equals(registration!.Course.Name)
            && registrationFind.StudentId == registration.StudentId);

        bool isFirstRegistration =
          registration!.RegistrationType.Equals(ERegistrationType.Primera.ToString())
          && registrationCount < 2;

        bool isSecondRegistration =
          registration.RegistrationType.Equals(ERegistrationType.Segunda.ToString())
          && registrationCount < 2;

        bool isContinuationRegistration =
          registration.RegistrationType.Equals(ERegistrationType.Continuación.ToString());

        return isFirstRegistration || isSecondRegistration || isContinuationRegistration;
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException("La inscripción no existe");
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
          .AsSplitQuery()
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
      ?? throw new NullReferenceException("Curso no encontrado");

      var course = _mapper.Map<CourseDto>(courseModel);

      if (courseModel.Professor != null)
      {
        course.Professor!.Email = courseModel.Professor.User.Email;
      }

      return course;
    }

    public async Task<CourseFileDto> GetCourseFile(int fileId)
    {
      CourseFile file = await _context.CourseFiles
        .FirstOrDefaultAsync(file => file.FileId == fileId)
        ?? throw new NullReferenceException("Archivo no encontrado");

      return _mapper.Map<CourseFileDto>(file);
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
        response = await CheckRepeatedFields(courseDto);
        var course = _mapper.Map<Course>(courseDto);

        if (file != null && file.Length > 0)
        {
          Response fileResponse = fileResponse = await _fileService.ValidateFile(file);
          response.Errors.AddRange(fileResponse.Errors);

          if (fileResponse.IsSuccess)
          {
            string path = await _fileService.SaveFile(file!, courseDto.Nrc, "files/courses");
            course.File = new CourseFile { Path = path };
          }
        }

        if (response.Errors.Count == 0)
        {
          course.IsOpen = IsCourseOpen(course.Period);
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
      try
      {
        foreach (var registration in registrations)
        {
          await GetCourse(registration.CourseId);
          await _studentService.GetStudent(registration.StudentId);
        }

        List<CourseRegistration> courseRegistrations = _mapper
          .Map<List<CourseRegistration>>(registrations);

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
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException(ex.Message);
      }
    }

    public async Task<Response> CreateProfessorAssignment(int courseId, int professorId)
    {
      try
      {
        var professor = await _professorService.GetProfessor(professorId);

        Course course = await _context.Courses.
          FirstOrDefaultAsync(course => course.CourseId == courseId)
          ?? throw new ArgumentNullException("Curso no encontrado");

        course.ProfessorId = professorId;
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al asignar el profesor");
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new ArgumentNullException(ex.Message);
      }
    }

    public async Task<Response> WithdrawCourseRegistration(int registrationId)
    {
      try
      {
        bool isValidCourseUnregistration = await IsValidCourseUnregistration(registrationId);

        if (isValidCourseUnregistration)
        {
          CourseRegistration registration = await _context.CourseRegistrations
            .FirstOrDefaultAsync(registration => registration.CourseRegistrationId == registrationId)
            ?? throw new NullReferenceException("Inscripción de curso no encontrada");

          registration.RegistrationType = ERegistrationType.Baja.ToString();
          _context.CourseRegistrations.Update(registration);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al retirar la inscripción");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException(ex.Message);
      }
    }

    public async Task<Response> UpdateCourse(CourseDto courseDto, IFormFile? file = null)
    {
      try
      {
        response = await CheckRepeatedFields(courseDto);
        Response fileResponse = new Response();
        string courseNrc = (await GetCourse(courseDto.CourseId)).Nrc;
        bool isCurrentNrc = courseNrc.Equals(courseDto.Nrc);
        bool isNrcTaken = isCurrentNrc
          ? false
          : response.Errors.Any(error => error.FieldName == "course.Nrc");

        if (file != null && file.Length > 0)
        {
          fileResponse = await _fileService.ValidateFile(file);
          response.Errors.AddRange(fileResponse.Errors);

          if (fileResponse.IsSuccess)
          {
            string path = await _fileService.SaveFile(file!, courseDto.Nrc, "files/courses");
            courseDto.File = new CourseFileDto { Path = path };
          }
        }

        if (!isNrcTaken && fileResponse.Errors.Count() == 0)
        {
          Course course = await _context.Courses
            .FirstOrDefaultAsync(course => course.CourseId == courseDto.CourseId)
            ?? throw new NullReferenceException("Curso no encontrado");

          if (fileResponse.IsSuccess) { course.File = _mapper.Map<CourseFile>(courseDto.File); }

          course.Section = courseDto.Section;
          course.Nrc = courseDto.Nrc;
          _context.Courses.Update(course);
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
        throw new OperationCanceledException("Ha ocurrido un error al crear el curso");
      }
    }

    public async Task<Response> DeleteCourseFile(int fileId)
    {
      try
      {
        CourseFile file = await _context.CourseFiles
          .FindAsync(fileId)
          ?? throw new NullReferenceException("Archivo no encontrado");

        _context.CourseFiles.Remove(file);
        await _context.SaveChangesAsync();
        _fileService.DeleteFile(file!.Path);
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al eliminar el archivo");
      }
    }
  }
}