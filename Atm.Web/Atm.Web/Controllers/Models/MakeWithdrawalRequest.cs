namespace Atm.Web.Controllers.Models;

public class MakeWithdrawalRequest
{
    [Range(1, 1000)]
    public int Amount { get; init; }
}