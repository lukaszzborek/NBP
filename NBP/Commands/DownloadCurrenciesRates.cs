using MediatR;
using NBP.Services;

namespace NBP.Commands;

public record DownloadCurrenciesRates(DateOnly Date) : IRequest;

public class DownloadCurrenciesRatesHandler : IRequestHandler<DownloadCurrenciesRates>
{
    private readonly ExchangeService _exchangeService;

    public DownloadCurrenciesRatesHandler(ExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }

    public async Task Handle(DownloadCurrenciesRates request, CancellationToken cancellationToken)
    {
        await _exchangeService.UpdateRates(request.Date);
    }
}