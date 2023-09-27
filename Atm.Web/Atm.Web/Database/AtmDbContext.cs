namespace Atm.Web.Database;

public class AtmDbContext : DbContext, IAtmDbContext
{
    public DbSet<Account> Accounts { get; init; }

    public DbSet<Session> Sessions { get; init; }

    public DbSet<Transaction> Transactions { get; init; }

    public AtmDbContext(DbContextOptions<AtmDbContext> options) : base(options)
    {
    }

    public void Add(Account account) => Accounts.Add(account);

    public void Add(Session session) => Sessions.Add(session);

    public void Add(Transaction transaction) => Transactions.Add(transaction);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AtmDbContext).Assembly);
    }

    IQueryable<Account> IAtmDbContext.Accounts => Accounts;

    IQueryable<Session> IAtmDbContext.Sessions => Sessions;

    IQueryable<Transaction> IAtmDbContext.Transactions => Transactions;
}
