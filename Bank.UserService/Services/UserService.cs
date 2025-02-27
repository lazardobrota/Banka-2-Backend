using System.IdentityModel.Tokens.Jwt;

using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.Application.Utilities;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;
using Bank.UserService.Security;

namespace Bank.UserService.Services;

public interface IUserService
{
    Task<Result<IEnumerable<UserResponse>>> GetAll(UserFilterQuery userFilterQuery, Pageable pageable);

    Task<Result<UserResponse>> GetOne(Guid id);

    Task<Result<TokenResponse>> Login(UserLoginRequest userLoginRequest);

    Task<Result> Activate(UserActivationRequest userActivationRequest, string token);

    Task<Result> PasswordReset(UserPasswordResetRequest userPasswordResetRequest, string token);
}

public class UserService(IUserRepository userRepository, TokenProvider tokenProvider) : IUserService
{
    private readonly IUserRepository m_UserRepository = userRepository;

    public async Task<Result<IEnumerable<UserResponse>>> GetAll(UserFilterQuery userFilterQuery, Pageable pageable)
    {
        var users = await m_UserRepository.FindAll(userFilterQuery, pageable);

        return Result.Ok(users.Select(user => user.ToResponse()));
    }

    public async Task<Result<UserResponse>> GetOne(Guid id)
    {
        var user = await m_UserRepository.FindById(id);

        if (user == null)
            return Result.NotFound<UserResponse>();

        return Result.Ok(user.ToResponse());
    }

    public async Task<Result<TokenResponse>> Login(UserLoginRequest userLoginRequest)
    {
        var user = await m_UserRepository.FindByEmail(userLoginRequest.Email);

        if (user == null)
            return Result.NotFound<TokenResponse>("User with the specified email address was not found.");

        if (!user.Activated)
            return Result.Forbidden<TokenResponse>("To proceed, the account has to be activated.");

        if (user.Password != HashingUtilities.HashPassword(userLoginRequest.Password, user.Salt))
            return Result.BadRequest<TokenResponse>("The password is incorrect.");

        string token = tokenProvider.Create(user);

        return Result.Ok(new TokenResponse { Token = token });
    }

    public async Task<Result> Activate(UserActivationRequest userActivationRequest, string token)
    {
        var handler  = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var id))
            return Result.BadRequest("Invalid token");

        if (userActivationRequest.Password != userActivationRequest.ConfirmPassword)
            return Result.BadRequest("Passwords do not match.");

        await m_UserRepository.SetPassword(id, userActivationRequest.Password);

        return Result.Accepted();
    }

    public async Task<Result> PasswordReset(UserPasswordResetRequest userPasswordResetRequest, string token)
    {
        return await Activate(new UserActivationRequest { Password = userPasswordResetRequest.Password, ConfirmPassword = userPasswordResetRequest.ConfirmPassword }, token);
    }
}
