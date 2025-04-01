namespace Bank.UserService.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    private const string c_PolicyPrefix = "Permission_";

    public AuthorizeAttribute() { }

    public new string Roles 
    { 
        get => base.Roles; 
        set => base.Roles = value; 
    }

    // Add a new Permissions property
    private string _permissions = string.Empty;
    public string Permissions
    {
        get => _permissions;
        set
        {
            _permissions = value;
            Policy       = $"{c_PolicyPrefix}{value}";
        }
    }
}