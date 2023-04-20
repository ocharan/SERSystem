using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

public interface ITokenService
{
  string GenerateToken(string userEmail);
  bool IsValidToken(string token);
}
