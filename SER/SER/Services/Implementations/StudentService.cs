using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using AutoMapper;
using SER.Models.DB;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.Responses;

namespace SER.Services
{
  public class StudentService : IStudentService
  {
    private readonly SERContext _context;
    private readonly IMapper _mapper;

    public StudentService(SERContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    private async Task<Response> CheckRepeatedFields(StudentDto studentDto)
    {
      try
      {
        Response response = new Response();

        bool isEnrollmentTaken = await _context.Students
          .AnyAsync(studentFind => studentFind.Enrollment
          .Equals(studentDto.Enrollment.Replace(" ", "")));

        bool isEmailTaken = await _context.Students
          .AnyAsync(studentFind => studentFind.Email
          .Equals(studentDto.Email.Replace(" ", "")));

        if (isEnrollmentTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "student.Enrollment",
            Message = "La matrícula ya está registrada"
          });
        }

        if (isEmailTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "student.Email",
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

    public IQueryable<StudentDto> GetAllStudents()
    {
      try
      {
        var students = _context.Students
          .ProjectTo<StudentDto>(_mapper.ConfigurationProvider);

        return students;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<StudentDto> GetStudent(int studentId)
    {
      Student student = await _context.Students.FindAsync(studentId)
        ?? throw new NullReferenceException("Alumno no encontrado");

      return _mapper.Map<StudentDto>(student);
    }

    public async Task<Response> CreateStudent(StudentDto studentDto)
    {
      try
      {
        Response response = await CheckRepeatedFields(studentDto);
        Student student = _mapper.Map<Student>(studentDto);

        if (response.Errors.Count == 0)
        {
          student.Enrollment = student.Enrollment.ToUpper();
          await _context.Students.AddAsync(student);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear el alumno");
      }
    }

    public async Task<Response> UpdateStudent(StudentDto studentDto)
    {
      try
      {
        Response response = await CheckRepeatedFields(studentDto);
        string studentEmail = (await GetStudent(studentDto.StudentId)).Email;
        bool isCurrentEmail = String.Equals(studentDto.Email, studentEmail);
        bool isEmailTaken = isCurrentEmail
          ? false
          : response.Errors.Any(error => error.FieldName.Equals("student.Email"));

        if (!isEmailTaken)
        {
          Student student = await _context.Students
            .FindAsync(studentDto.StudentId)
            ?? throw new NullReferenceException("Alumno no encontrado");

          student!.FullName = studentDto.FullName;
          student.Email = studentDto.Email;
          _context.Students.Update(student);
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
        throw new Exception("Ha ocurrido un error al actualizar el alumno");
      }
    }

    public async Task<List<StudentDto>> SearchStudent(string search)
    {
      var students = await _context.Students
        .Where(student => student.FullName.Contains(search)
          || student.Email.Contains(search)
          || student.Enrollment.Contains(search))
        .Where(student => !_context.CourseRegistrations
          .Where(enrollment => enrollment.StudentId == student.StudentId)
          .Any(enrollment => _context.Courses
            .Where(course => course.CourseId == enrollment.CourseId)
            .Any(course => course.IsOpen)))
        .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

      return students;
    }

    public async Task<bool> IsStudentExisting(int studentId)
    {
      return await _context.Students
        .AnyAsync(student => student.StudentId == studentId);
    }
  }
}