using System.Security.Claims;
using System.Text;

using Bank.Application;
using Bank.UserService.Application;
using Bank.UserService.Models;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bank.UserService.Security;

public class TokenProvider
{
    public string Create(User user)
    {
        int expirationInMinutes = Convert.ToInt32(Environment.GetEnvironmentVariable(EnvironmentVariable.ExpirationInMinutes) ?? EnvironmentVariable.ExpirationInMinutesElseValue);
        string secretKey = Environment.GetEnvironmentVariable(EnvironmentVariable.SecretKey) ?? EnvironmentVariable.SecretKeyElseValue;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
                     {
                         new ("id",   user.Id.ToString()),
                         new ("auth", user.Role.ToString())
                     };

        var tokenDescriptor = new SecurityTokenDescriptor
                              {
                                  Subject            = new ClaimsIdentity(claims),
                                  Expires            = DateTime.Now.AddMinutes(expirationInMinutes),
                                  SigningCredentials = credentials
                              };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }
}
