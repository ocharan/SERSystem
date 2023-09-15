using SER.Models.DTO;
using SER.Models.Responses;

namespace SER.Services
{
  public interface IUserService
  {
    Task<string> GetUserEmailById(int userId);
    Task<UserDto> GetUserByUsername(string username);
    Task<bool> IsValidUserToken(string token);
    Task<string> GenerateRecoveryToken(string email);
    Task<bool> UpdateUserPassword(string username, string password);
  }
}