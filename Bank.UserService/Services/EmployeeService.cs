using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IEmployeeService
{
    Task<Result<List<EmployeeResponse>>> GetAll(UserFilterQuery userFilterQuery, Pageable pageable);
    Task<Result<EmployeeResponse>> GetOne(Guid id);

    Task<Result<EmployeeResponse>> Create(EmployeeCreateRequest employeeCreateRequest);

    Task<Result<EmployeeResponse>> Update(EmployeeUpdateRequest employeeUpdateRequest, Guid id);
    
}

public class EmployeeService(IUserRepository userRepository) : IEmployeeService
{
    private readonly IUserRepository m_UserRepository = userRepository;

    public async Task<Result<List<EmployeeResponse>>> GetAll(UserFilterQuery userFilterQuery, Pageable pageable)
    {
        var users = await m_UserRepository.FindAll(userFilterQuery, pageable);

        if (users.Count == 0)
            return Result.NoContent<List<EmployeeResponse>>();

        return Result.Ok(users.Select(user => user.ToEmployee()
                                                  .ToResponse())
                              .ToList());
    }
    
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

    public async Task<Result<EmployeeResponse>> Update(EmployeeUpdateRequest employeeUpdateRequest, Guid id)
    {
        var oldUser = await m_UserRepository.FindById(id);

        if (oldUser is null)
            return Result.NotFound<EmployeeResponse>($"No Employee found with Id: {id}");

        var user = await m_UserRepository.Update(oldUser, employeeUpdateRequest.ToEmployee(oldUser.ToEmployee())
                                                                               .ToUser());

        return Result.Ok(user.ToEmployee()
                             .ToResponse());
    }
}
