using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NBP.EF.Models;

namespace NBP.EF;

public class CurrencyDbContext : DbContext
{
    public DbSet<CurrencyRate> CurrencyRates { get; set; }

    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
                            .HaveConversion<DateOnlyConverter>()
                            .HaveColumnType("date");
    }
}

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    /// <summary>
    ///     Creates a new instance of this converter.
    /// </summary>
    public DateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d))
    {
    }
}