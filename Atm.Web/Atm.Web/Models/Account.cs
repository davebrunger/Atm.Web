namespace Atm.Web.Models;

public class Account
{
    public string AccountNumber { get; init; } = null!;

    public string Pin { get; init; } = null!;

    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; init; } = new List<Transaction>();
    
    public ICollection<Session> Sessions { get; init; } = new List<Session>();
}
