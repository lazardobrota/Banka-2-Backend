using Bank.Application.Domain;

namespace Bank.Application.Requests;

public class UpdatePermissionsRequest
{
    public required PermissionOperation Operation   { get; set; }
    public required Permission          Permissions { get; set; }
}
