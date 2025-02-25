using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.Application.Utilities;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IUserService
{
    Task<Result<IEnumerable<UserResponse>>> GetAll(UserFilterQuery userFilterQuery, Pageable pageable);

    Task<Result<UserResponse>> GetOne(Guid id);

    Task<Result<UserResponse>> Login(UserLoginRequest userLoginRequest);
}

public class UserService(IUserRepository userRepository) : IUserService
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

    public async Task<Result<UserResponse>> Login(UserLoginRequest userLoginRequest)
    {
        var user = await m_UserRepository.FindByEmail(userLoginRequest.Email);

        if (user == null)
            return Result.NotFound<UserResponse>("User with the specified email address was not found.");

        if (!user.Activated)
            return Result.Forbidden<UserResponse>("To proceed, the account has to be activated.");

        if (user.Password != HashingUtilities.HashPassword(userLoginRequest.Password, user.Salt))
            return Result.BadRequest<UserResponse>("The password is incorrect.");

        return Result.Ok(user.ToResponse());
    }
}
