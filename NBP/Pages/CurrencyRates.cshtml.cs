using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NBP.Commands;
using NBP.EF.Models;
using NBP.Kernel;
using NBP.Queries;

namespace NBP.Pages;

public class CurrencyRates : PageModel
{
    private readonly IMediator _mediator;
    private readonly ILogger<CurrencyRates> _logger;

    public IList<CurrencyRate> CurrencyRate { get; set; } = new List<CurrencyRate>();
    public string ErrorMessage { get; set; }
    public DateOnly Date { get; set; }

    public CurrencyRates(IMediator mediator,
        IClock clock,
        ILogger<CurrencyRates> logger)
    {
        _mediator = mediator;
        _logger = logger;
        Date = DateOnly.FromDateTime(clock.GetUtcNow());
    }

    public async Task OnGetAsync()
    {
        await GetCurrencyRatesAsync();
    }

    private async Task GetCurrencyRatesAsync()
    {
        ErrorMessage = string.Empty;
        try
        {
            CurrencyRate = await _mediator.Send(new GetCurrencyRatesByDate());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while downloading currency rates");
            ErrorMessage = "Failed to download currency rates";
        }
    }

    public async Task OnGetOnClick()
    {
        ErrorMessage = string.Empty;
        try
        {
            await _mediator.Send(new DownloadCurrenciesRates(Date));
            await GetCurrencyRatesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while downloading currency rates");
            ErrorMessage = "Failed to download currency rates";
        }
    }
}