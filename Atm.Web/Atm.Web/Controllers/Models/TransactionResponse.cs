namespace Atm.Web.Controllers.Models;

public class TransactionResponse : AccountRespose
{
    public long Id { get; init; }

    public DateTime DateTime { get; init; }

    public decimal Amount { get; init; }
}