using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBP.EF.Models;

namespace NBP.EF.Configurations;

public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
{
    public void Configure(EntityTypeBuilder<CurrencyRate> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .HasIndex(x => new { x.CurrencyCode, x.Date, x.Table })
            .IsUnique();

        builder
            .Property(x => x.Table)
            .IsRequired()
            .HasMaxLength(1);

        builder
            .Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(5);

        builder
            .Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(x => x.Date)
            .IsRequired();

        builder
            .Property(x => x.Rate)
            .HasPrecision(30, 8);
    }
}