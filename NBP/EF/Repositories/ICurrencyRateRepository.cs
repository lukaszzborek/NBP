using Microsoft.EntityFrameworkCore;
using NBP.EF.Models;

namespace NBP.EF.Repositories;

public interface ICurrencyRateRepository
{
    Task<CurrencyRate?> GetAsync(string currencyCode, DateOnly date, string table);
    void Add(CurrencyRate currencyRate);
    void Update(CurrencyRate currencyRate);
    Task SaveChangesAsync();
}

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly CurrencyDbContext _currencyDbContext;

    public CurrencyRateRepository(CurrencyDbContext currencyDbContext)
    {
        _currencyDbContext = currencyDbContext;
    }

    public Task<CurrencyRate?> GetAsync(string currencyCode, DateOnly date, string table)
    {
        return _currencyDbContext
               .CurrencyRates
               .Where(x => x.CurrencyCode == currencyCode
                           && x.Date == date
                           && x.Table == table)
               .FirstOrDefaultAsync();
    }

    public void Add(CurrencyRate currencyRate)
    {
        _currencyDbContext.CurrencyRates.Add(currencyRate);
    }

    public void Update(CurrencyRate currencyRate)
    {
        _currencyDbContext.CurrencyRates.Update(currencyRate);
    }

    public async Task SaveChangesAsync()
    {
        await _currencyDbContext.SaveChangesAsync();
    }
}