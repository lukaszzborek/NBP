using NBP.Clients;
using NBP.Clients.DTO;
using NBP.EF.Models;
using NBP.EF.Repositories;

namespace NBP.Services;

public class ExchangeService
{
    private readonly ICurrencyExchangeClient _currencyExchangeClient;
    private readonly ICurrencyRateRepository _currencyRateRepository;

    public ExchangeService(ICurrencyExchangeClient currencyExchangeClient,
        ICurrencyRateRepository currencyRateRepository)
    {
        _currencyExchangeClient = currencyExchangeClient;
        _currencyRateRepository = currencyRateRepository;
    }

    public async Task UpdateRates(DateOnly date)
    {
        var ratesA = await _currencyExchangeClient.GetCurrencyExchangeAsync("A", date);
        // ratesB are only updated once a week
        var ratesB = await _currencyExchangeClient.GetCurrencyExchangeAsync("B");

        await UpdateRates(new List<CurrencyExchangeDto> { ratesA, ratesB });
    }

    public async Task UpdateRates(List<CurrencyExchangeDto> currencyExchangeDtos)
    {
        foreach (var currencyExchangeDto in currencyExchangeDtos)
        {
            foreach (var rate in currencyExchangeDto.Rates)
            {
                var currencyRate = await _currencyRateRepository.GetAsync(rate.Code, currencyExchangeDto.EffectiveDate,
                    currencyExchangeDto.Table);

                if (currencyRate is null)
                {
                    currencyRate = new CurrencyRate(rate.Code, rate.Currency, currencyExchangeDto.EffectiveDate,
                        currencyExchangeDto.Table);
                    currencyRate.SetRate(rate.Mid);

                    _currencyRateRepository.Add(currencyRate);
                }
                else
                {
                    currencyRate.SetRate(rate.Mid);
                    _currencyRateRepository.Update(currencyRate);
                }
            }
        }

        await _currencyRateRepository.SaveChangesAsync();
    }
}