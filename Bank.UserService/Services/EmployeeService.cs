using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IEmployeeService
{
    Task<Result<EmployeeResponse>> GetOne(Guid id);

    Task<Result<EmployeeResponse>> Create(EmployeeCreateRequest employeeCreateRequest);
}

public class EmployeeService(IUserRepository userRepository) : IEmployeeService
{
    private readonly IUserRepository m_UserRepository = userRepository;

    public async Task<Result<EmployeeResponse>> GetOne(Guid id)
    {
        var user = await m_UserRepository.FindById(id);

        if (user == null)
            return Result.NotFound<EmployeeResponse>($"No Employee found with Id: {id}");

        return Result.Ok(user.ToEmployee()
                             .ToResponse());
    }

    public async Task<Result<EmployeeResponse>> Create(EmployeeCreateRequest employeeCreateRequest)
    {
        var user = await m_UserRepository.Add(employeeCreateRequest.ToEmployee()
                                                                   .ToUser());

        return Result.Ok(user.ToEmployee()
                             .ToResponse());
    }
}
