namespace Atm.Web.Database;

public static class DataSeeder
{
    public static async Task SeedDataAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AtmDbContext>();

        context.Database.EnsureCreated();

        var testAccount = await context.Accounts.SingleOrDefaultAsync(a => a.AccountNumber == "1234567890987654", cancellationToken);
        if (testAccount == null)
        {
            context.Accounts.Add(new Account { AccountNumber = "1234567890987654", Pin = "1234", Balance = 1000000 });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
