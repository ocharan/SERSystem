using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using AutoMapper;
using SER.Models.DB;
using AutoMapper.QueryableExtensions;
using SER.Configuration;

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

    private IQueryable<StudentDto> GetAssignedStudents()
    {
      try
      {
        var students = _context.Students
          .Where(student => _context.CourseRegistrations
          .Any(registration => (registration.StudentId == student.StudentId) && _context.Courses
          .Any(course => (course.CourseId == registration.CourseId) && course.IsOpen)));

        return students.ProjectTo<StudentDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private IQueryable<StudentDto> GetUnassignedStudents()
    {
      try
      {
        var students = _context.Students
          .Where(student => !_context.CourseRegistrations
          .Any(registration => String.Equals(registration.StudentId, student.StudentId) && _context.Courses
          .Any(course => String.Equals(course.CourseId, registration.CourseId) && course.IsOpen)));

        return students.ProjectTo<StudentDto>(_mapper.ConfigurationProvider);
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    private Dictionary<string, bool> IsStudentExisting(StudentDto studentDto)
    {
      try
      {
        Dictionary<string, bool> isTaken = new Dictionary<string, bool>();

        isTaken.Add("IsEnrollmentTaken", _context.Students
          .Any(studentFind => String.Equals(studentFind.Enrollment, studentDto.Enrollment.Replace(" ", ""))));

        isTaken.Add("IsEmailTaken", _context.Students
          .Any(studentFind => String.Equals(studentFind.Email, studentDto.Email.Replace(" ", ""))));

        return isTaken;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public (IQueryable<StudentDto> Students, int AssignedCount, int UnassignedCount) GetAllStudents(string filter)
    {
      IQueryable<StudentDto> students = new List<StudentDto>().AsQueryable();

      try
      {
        switch (filter)
        {
          case "assigned":
            students = GetAssignedStudents();

            return (
              students,
              students.Count(),
              GetUnassignedStudents().Count()
            );

          case "unassigned":
            students = GetUnassignedStudents();

            return (
              students,
              GetAssignedStudents().Count(),
              students.Count()
            );

          default:
            students = GetAssignedStudents().Concat(GetUnassignedStudents());

            return (
              students,
              GetAssignedStudents().Count(),
              GetUnassignedStudents().Count()
            );
        }
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

    public async Task<Dictionary<string, bool>> CreateStudent(StudentDto studentDto)
    {
      try
      {
        Student student = _mapper.Map<Student>(studentDto);
        Dictionary<string, bool> isTaken = IsStudentExisting(studentDto);
        bool isEnrollmentTaken = isTaken["IsEnrollmentTaken"];
        bool isEmailTaken = isTaken["IsEmailTaken"];

        if (!isEnrollmentTaken && !isEmailTaken)
        {
          student.Enrollment = student.Enrollment.ToUpper();
          await _context.Students.AddAsync(student);
          await _context.SaveChangesAsync();
          isTaken.Add("IsCreated", true);
        }

        return isTaken;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear el alumno");
      }
    }

    public async Task<Dictionary<string, bool>> UpdateStudent(StudentDto studentDto)
    {
      try
      {
        Dictionary<string, bool> isTaken = IsStudentExisting(studentDto);
        string studentEmail = (await GetStudent(studentDto.StudentId)).Email;
        bool isCurrentEmail = String.Equals(studentDto.Email, studentEmail);
        bool isEmailTaken = isCurrentEmail ? false : isTaken["IsEmailTaken"];

        if (!isEmailTaken)
        {
          Student student = await _context.Students
            .FindAsync(studentDto.StudentId)
            ?? throw new NullReferenceException("Alumno no encontrado");
          
          student!.FullName = studentDto.FullName;
          student.Email = studentDto.Email;
          _context.Students.Update(student);
          await _context.SaveChangesAsync();
          isTaken.Add("IsUpdated", true);
          isTaken["IsEmailTaken"] = false;
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
        throw new Exception("Ha ocurrido un error al actualizar el alumno");
      }
    }
  }
}