namespace Atm.Web.Database.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.AccountNumber).HasMaxLength(16);

        builder.Property(a => a.Amount).HasColumnType("MONEY");
    }
}
