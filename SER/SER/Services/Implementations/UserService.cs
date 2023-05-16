using Microsoft.EntityFrameworkCore;
using SER.Models.DB;
using SER.Models.DTO;
using AutoMapper;
using SER.Configuration;

namespace SER.Services
{
  public class UserService : IUserService
  {
    private readonly SERContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public UserService(SERContext context, IMapper mapper, ITokenService tokenService)
    {
      _context = context;
      _mapper = mapper;
      _tokenService = tokenService;
    }

    public async Task<UserDto> GetUserByUsername(string username)
    {
      User user = await _context.Users
        .SingleOrDefaultAsync(userFind => userFind.Username.Equals(username))
        ?? throw new NullReferenceException("Usuario no encontrado");

      return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> IsValidUserToken(string token)
    {
      try
      {
        bool isTokenFound = await _context.Users
          .AnyAsync(userFind => userFind.RecoveryToken!.Equals(token));

        return _tokenService.IsValidToken(token) && isTokenFound;
      }
      catch (ArgumentNullException ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<string> GenerateRecoveryToken(string email)
    {
      try
      {
        User user = await _context.Users
          .SingleOrDefaultAsync(userFind => userFind.Email.Equals(email))
          ?? throw new NullReferenceException();

        user.RecoveryToken = _tokenService.GenerateToken(user.Email);
        await _context.SaveChangesAsync();

        return user.RecoveryToken;
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }

    public async Task<bool> UpdateUserPassword(string token, string password)
    {
      try
      {
        User user = await _context.Users
          .SingleOrDefaultAsync(userFind => String.Equals(userFind.RecoveryToken, token))
          ?? throw new NullReferenceException("Usuario no encontrado");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user!.Password = passwordHash;
        user.RecoveryToken = null;
        await _context.SaveChangesAsync();

        return true;
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        throw;
      }
    }
  }
}