namespace Atm.Web.Controllers.Models;

public class LoginResponse : AccountRespose
{
    public string Token { get; init; } = null!;
}