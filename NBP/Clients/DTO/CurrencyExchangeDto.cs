namespace NBP.Clients.DTO;

public record CurrencyExchangeDto(string Table, string No, DateOnly EffectiveDate, List<CurrencyRateDto> Rates);