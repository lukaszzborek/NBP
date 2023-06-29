using System.Net;
using NBP.Clients.DTO;
using NBP.Clients.Exceptions;
using Polly;
using Polly.Retry;

namespace NBP.Clients;

public class NBPCurrencyExchangeClient : ICurrencyExchangeClient
{
    private const string TablesEndpoint = "exchangerates/tables";
    private static readonly HashSet<string> _validTables = new() { "A", "B" };

    private readonly HttpClient _httpClient;

    private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy =
        Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadRequest)
            .RetryAsync(3);

    public NBPCurrencyExchangeClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CurrencyExchangeDto> GetCurrencyExchangeAsync(string table, DateOnly date)
    {
        if (!_validTables.Contains(table))
            throw new InvalidNBPTableException(table);

        var response =
            await retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{TablesEndpoint}/{table}/{date:yyyy-MM-dd}"));
        if (!response.IsSuccessStatusCode)
            return new CurrencyExchangeDto(string.Empty, string.Empty, date, new List<CurrencyRateDto>());
        var currencyExchange = await response.Content.ReadFromJsonAsync<List<CurrencyExchangeDto>>();
        if (currencyExchange is null)
            return new CurrencyExchangeDto(string.Empty, string.Empty, date, new List<CurrencyRateDto>());

        return currencyExchange[0];
    }

    public async Task<CurrencyExchangeDto> GetCurrencyExchangeAsync(string table)
    {
        if (!_validTables.Contains(table))
            throw new InvalidNBPTableException(table);

        var response = await retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{TablesEndpoint}/{table}"));
        if (!response.IsSuccessStatusCode)
            return new CurrencyExchangeDto(string.Empty, string.Empty, DateOnly.MinValue, new List<CurrencyRateDto>());
        var currencyExchange = await response.Content.ReadFromJsonAsync<List<CurrencyExchangeDto>>();
        if (currencyExchange is null)
            return new CurrencyExchangeDto(string.Empty, string.Empty, DateOnly.MinValue, new List<CurrencyRateDto>());

        return currencyExchange[0];
    }
}