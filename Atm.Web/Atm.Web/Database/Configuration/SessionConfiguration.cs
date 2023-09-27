namespace Atm.Web.Database.Configuration;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.AccountNumber).HasMaxLength(16);
    }
}
