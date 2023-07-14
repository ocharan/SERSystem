using Microsoft.EntityFrameworkCore;
using SER.Models.DTO;
using SER.Models.DB;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SER.Configuration;
using SER.Models.Responses;
using SER.Models.Enums;
using SER.Services.Contracts;

namespace SER.Services.Implementations
{
  public class AcademicBodyService : IAcademicBodyService
  {
    public readonly SERContext _context;
    public readonly IMapper _mapper;
    public readonly IProfessorService _professorService;
    public Response response = new Response();

    public AcademicBodyService(SERContext context, IMapper mapper, IProfessorService professorService)
    {
      _context = context;
      _mapper = mapper;
      _professorService = professorService;
    }

    private async Task<Response> CheckRepeatedFields(AcademicBodyDto academicBodyDto)
    {
      try
      {
        bool isKeyTaken = await _context.AcademicBodies
          .AnyAsync(academicBody => academicBody.AcademicBodyKey == academicBodyDto.AcademicBodyKey);

        if (isKeyTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "academicBody.AcademicBodyKey",
            Message = "La clave del cuerpo académico ya está en uso"
          });
        }

        bool isNameTaken = await _context.AcademicBodies
          .AnyAsync(academicBody => academicBody.Name == academicBodyDto.Name);

        if (isNameTaken)
        {
          response.Errors.Add(new FieldError
          {
            FieldName = "academicBody.Name",
            Message = "El nombre del cuerpo académico ya está en uso"
          });
        }

        return response;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException();
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException();
      }
    }

    public async Task<AcademicBodyDto> GetAcademicBody(int academicBodyId)
    {
      AcademicBody academicBody = await _context.AcademicBodies
        .Include(academicBody => academicBody.AcademicBodyLgacs)
          .ThenInclude(academicBodyLgac => academicBodyLgac.Lgac)
        .Include(academicBody => academicBody.AcademicBodyMembers)
          .ThenInclude(academicBodyMember => academicBodyMember.Professor)
        .AsSplitQuery()
        .FirstOrDefaultAsync(academicBody => academicBody.AcademicBodyId == academicBodyId)
        ?? throw new NullReferenceException("Cuerpo Académico no encontrado");

      return _mapper.Map<AcademicBodyDto>(academicBody);
    }

    public IQueryable<AcademicBodyDto> GetAllAcademicBodies()
    {
      var academicBodies = _context.AcademicBodies
        .AsSplitQuery()
        .ProjectTo<AcademicBodyDto>(_mapper.ConfigurationProvider);

      return academicBodies;
    }

    public async Task<Response> CreateAcademicBody(AcademicBodyDto academicBodyDto)
    {
      try
      {
        response = await CheckRepeatedFields(academicBodyDto);
        var academicBody = _mapper.Map<AcademicBody>(academicBodyDto);

        if (response.Errors.Count == 0)
        {
          await _context.AcademicBodies.AddAsync(academicBody);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear el Cuerpo Académico");
      }
    }

    public async Task<Response> UpdateAcademicBody(AcademicBodyDto academicBodyDto)
    {
      try
      {
        response = await CheckRepeatedFields(academicBodyDto);

        string academicBodyKey = (await GetAcademicBody(academicBodyDto.AcademicBodyId)).AcademicBodyKey;
        bool isCurrentKey = academicBodyKey.Equals(academicBodyDto.AcademicBodyKey);
        bool isTakenKey = isCurrentKey
          ? false
          : response.Errors.Any(error => error.FieldName == "academicBody.AcademicBodyKey");

        string academicBodyName = (await GetAcademicBody(academicBodyDto.AcademicBodyId)).Name;
        bool isCurrentName = academicBodyName.Equals(academicBodyDto.Name);
        bool isTakenName = isCurrentName
          ? false
          : response.Errors.Any(error => error.FieldName == "academicBody.Name");

        System.Console.WriteLine($"Error count: {response.Errors.Count}");

        if (!isTakenKey && !isTakenName)
        {
          AcademicBody academicBody = await _context.AcademicBodies
            .FirstOrDefaultAsync(academicBody => academicBody.AcademicBodyId == academicBodyDto.AcademicBodyId)
            ?? throw new NullReferenceException("Cuerpo Académico no encontrado");

          academicBody.AcademicBodyKey = academicBodyDto.AcademicBodyKey;
          academicBody.Name = academicBodyDto.Name;
          academicBody.Ies = academicBodyDto.Ies;
          academicBody.ConsolidationDegree = academicBodyDto.ConsolidationDegree;
          academicBody.Discipline = academicBodyDto.Discipline;

          _context.AcademicBodies.Update(academicBody);
          await _context.SaveChangesAsync();
          response.IsSuccess = true;
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

    public async Task<Response> CreateLgac(LgacDto lgacDto)
    {
      try
      {
        await GetAcademicBody(lgacDto.AcademicBodyId);
        int academicBodyId = lgacDto.AcademicBodyId;
        var lgac = _mapper.Map<Lgac>(lgacDto);

        lgac.AcademicBodyLgacs = new List<AcademicBodyLgac>
        {
          new AcademicBodyLgac
          {
            AcademicBodyId = academicBodyId,
          }
        };

        await _context.Lgacs.AddAsync(lgac);
        await _context.SaveChangesAsync();
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear la LGAC");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException(ex.Message);
      }
    }

    public async Task<Response> UpdateLgac(LgacDto lgacDto)
    {
      try
      {
        await GetAcademicBody(lgacDto.AcademicBodyId);
        var lgac = _context.Lgacs
          .FirstOrDefault(lgac => lgac.LgacId == lgacDto.LgacId)
          ?? throw new NullReferenceException("LGAC no encontrada");

        lgac.Name = lgacDto.Name;
        lgac.Description = lgacDto.Description;

        _context.Lgacs.Update(lgac);
        await _context.SaveChangesAsync();
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al actualizar la LGAC");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException(ex.Message);
      }
    }

    public async Task<Response> DeleteLgac(int lgacId)
    {
      try
      {
        var academicBodyLgac = _context.AcademicBodyLgacs
          .FirstOrDefault(academicBodyLgac => academicBodyLgac.LgacId == lgacId)
          ?? throw new NullReferenceException("LGAC no encontrada");

        _context.AcademicBodyLgacs.Remove(academicBodyLgac);
        await _context.SaveChangesAsync();
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al eliminar la LGAC");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException(ex.Message);
      }
    }

    public async Task<Response> CreateAcademicBodyMembers(List<AcademicBodyMemberDto> members)
    {
      try
      {
        List<int> professorsId = _context.AcademicBodyMembers
          .Select(academicBodyMember => academicBodyMember.ProfessorId)
          .ToList();

        foreach (var profesor in members)
        {
          await GetAcademicBody(profesor.AcademicBodyId);
          await _professorService.GetProfessor(profesor.ProfessorId);

          if (professorsId.Contains(profesor.ProfessorId))
          {
            response.Errors.Add(new FieldError
            {
              FieldName = "professor",
              Message = "El profesor ya es miembro de un Cuerpo Académico"
            });
          }
        }

        if (response.Errors.Count == 0)
        {
          var academicBodyMembers = _mapper.
            Map<List<AcademicBodyMember>>(members);

          await _context.AcademicBodyMembers.AddRangeAsync(academicBodyMembers);
          await _context.SaveChangesAsync();
        }

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al crear los miembros del Cuerpo Académico");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException(ex.Message);
      }
    }

    public async Task<Response> DeleteMember(int memberId)
    {
      try
      {
        AcademicBodyMember member = await _context.AcademicBodyMembers
          .FirstOrDefaultAsync(member => member.AcademicBodyMemberId == memberId)
          ?? throw new NullReferenceException("Miembro no encontrado");

        _context.AcademicBodyMembers.Remove(member);
        await _context.SaveChangesAsync();
        response.IsSuccess = true;

        return response;
      }
      catch (OperationCanceledException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new OperationCanceledException("Ha ocurrido un error al retirar el miembro del Cuerpo Académico");
      }
      catch (NullReferenceException ex)
      {
        ExceptionLogger.LogException(ex);
        throw new NullReferenceException(ex.Message);
      }
    }
  }
}