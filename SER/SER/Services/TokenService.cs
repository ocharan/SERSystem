using SER.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace SER.Services
{
  public class TokenService : ITokenService
  {
    private readonly TokenSettings _tokenSettings;

    public TokenService(IOptions<TokenSettings> tokenSettings)
    {
      _tokenSettings = tokenSettings.Value;
    }

    public string GenerateToken(string userEmail)
    {
      byte[] key = Encoding.UTF8.GetBytes(_tokenSettings.Secret!);
      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.Email, userEmail)
        }),
        Expires = DateTime.UtcNow.AddMinutes(20),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature
        )
      };

      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public bool IsValidToken(string token)
    {
      byte[] key = Encoding.UTF8.GetBytes(_tokenSettings.Secret!);
      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

      TokenValidationParameters validationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
      };

      try
      {
        SecurityToken validatedToken = new JwtSecurityToken();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(
          token, validationParameters, out validatedToken
        );

        return true;
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);

        return false;
      }
    }
  }
}