using NBP.Kernel;
using NBP.Services;

namespace NBP.HostedService;

public class CurrencyExchangeHostedService : BackgroundService
{
    private readonly ILogger<CurrencyExchangeHostedService> _logger;
    private readonly IServiceProvider _services;

    public CurrencyExchangeHostedService(IServiceProvider services,
        ILogger<CurrencyExchangeHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
            try
            {
                using var scope = _services.CreateScope();

                var exchangeService = scope.ServiceProvider.GetRequiredService<ExchangeService>();
                var clock = scope.ServiceProvider.GetRequiredService<IClock>();

                await exchangeService.UpdateRates(DateOnly.FromDateTime(clock.GetUtcNow()));
                _logger.LogInformation("Currency rates updated");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update currency rates: {CallStack}", e.ToString());
            }
    }
}