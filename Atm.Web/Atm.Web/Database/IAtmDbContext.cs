namespace Atm.Web.Database;

public interface IAtmDbContext : IDisposable
{
    IQueryable<Account> Accounts { get; }
    IQueryable<Session> Sessions { get; }
    IQueryable<Transaction> Transactions { get; }

    void Add(Account account);

    void Add(Session session);

    void Add(Transaction transaction);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
