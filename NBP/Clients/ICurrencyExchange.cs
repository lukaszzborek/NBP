using NBP.Clients.DTO;

namespace NBP.Clients;

public interface ICurrencyExchangeClient
{
    public Task<CurrencyExchangeDto> GetCurrencyExchangeAsync(string table, DateOnly date);
    public Task<CurrencyExchangeDto> GetCurrencyExchangeAsync(string table);
}