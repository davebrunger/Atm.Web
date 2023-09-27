namespace Atm.Web.Models;

public class Transaction
{
    public long Id { get; init; }

    public string AccountNumber { get; init; } = null!;

    public DateTime DateTime { get; init; }

    public decimal Amount { get; init; }

    public Account Account { get; init; } = null!;
}
