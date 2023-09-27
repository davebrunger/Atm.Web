namespace Atm.Web.Models;

public class Session
{
    public long Id { get; set; }

    public string AccountNumber { get; init; } = null!;

    public DateTime StartedAtUtc { get; init; }

    public DateTime LastActivityAtUtc { get; set; }
    
    public DateTime? FinishedAtUtc { get; set; }

    public Account Account { get; init; } = null!;
}