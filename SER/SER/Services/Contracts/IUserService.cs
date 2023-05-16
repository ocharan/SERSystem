using SER.Models.DTO;

namespace SER.Services
{
  public interface IUserService
  {
    Task<UserDto> GetUserByUsername(string username);
    Task<bool> IsValidUserToken(string token);
    Task<string> GenerateRecoveryToken(string email);
    Task<bool> UpdateUserPassword(string username, string password);
  }
}