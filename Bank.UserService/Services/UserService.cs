using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IUserService
{
    Task<Result<UserResponse>> GetOne(Guid id);
}

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository m_UserRepository = userRepository;

    public async Task<Result<UserResponse>> GetOne(Guid id)
    {
        var user = await m_UserRepository.FindById(id);

        if (user == null)
            return Result.NotFound<UserResponse>();

        return Result.Ok(user.ToResponse());
    }
}
