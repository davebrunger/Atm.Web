namespace Atm.Web.Controllers.Models;

public class GetBalanceResponse : AccountRespose
{
    public decimal Balance { get; init; }
}