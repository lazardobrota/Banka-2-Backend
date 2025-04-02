using System.Security.Claims;
using System.Text;

using Bank.Application.Domain;
using Bank.UserService.Configurations;
using Bank.UserService.Models;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bank.UserService.Services;

public interface IAuthorizationService
{
    public Guid UserId { get; }
    public Role Role   { get; }

    public string GenerateToken();

    public string GenerateTokenFor(User user);
}

public class AuthorizationService : IAuthorizationService
{
    public Guid       UserId      { get; }
    public Role       Role        { get; }
    public Permission Permissions { get; }

    public AuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        var userId     = httpContextAccessor.HttpContext?.User.FindFirst("id");
        var role       = httpContextAccessor.HttpContext?.User.FindFirst("role");
        var permission = httpContextAccessor.HttpContext?.User.FindFirst("permission");

        UserId = userId != null ? Guid.Parse(userId.Value) : Guid.Empty;
        Role   = role   != null ? Enum.TryParse(role.Value, out Role myRole) ? myRole : Role.Invalid : Role.Invalid;
        //Permissions = permission != null ? Enum.TryParse(permission.Value, out Permission myPermissions) ? myPermissions : Permission.Invalid : Permission.Invalid;
        Permissions = permission != null ? (Permission)long.Parse(permission.Value) : Permission.Invalid;
    }

    public string GenerateToken() => GenerateToken(UserId, Role, Permissions);

    public string GenerateTokenFor(User user) => GenerateToken(user.Id, user.Role, user.Permissions);

    private static string GenerateToken(Guid userId, Role role, Permission permission)
    {
        var expirationInMinutes = Configuration.Jwt.ExpirationTimeInMinutes;
        var securityKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.Jwt.SecretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
                     {
                         new("id", userId.ToString()),
                         new("role", role.ToString()),
                         //new("permission", permission.ToString())
                         new("permission", ((long)permission).ToString())
                     };

        var tokenDescriptor = new SecurityTokenDescriptor
                              {
                                  Subject            = new ClaimsIdentity(claims),
                                  Expires            = DateTime.Now.AddMinutes(expirationInMinutes),
                                  SigningCredentials = credentials
                              };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}
