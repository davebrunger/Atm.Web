namespace Atm.Web.Database.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.AccountNumber);
        builder.Property(a => a.AccountNumber).HasMaxLength(16);

        builder.Property(a => a.Pin).HasMaxLength(4);

        builder.Property(a => a.Balance).HasColumnType("MONEY");

        builder
            .HasMany(a => a.Sessions)
            .WithOne(s => s.Account)
            .HasForeignKey(s => s.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
