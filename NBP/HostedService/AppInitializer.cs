using Microsoft.EntityFrameworkCore;

namespace NBP.HostedService;

internal class AppInitializer : IHostedService
{
    private readonly ILogger<AppInitializer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AppInitializer(IServiceProvider serviceProvider, ILogger<AppInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
                                      .SelectMany(x => x.GetTypes())
                                      .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsInterface &&
                                                  x != typeof(DbContext));

        using var scope = _serviceProvider.CreateScope();
        foreach (var dbContextType in dbContextTypes)
        {
            var dbContext = scope.ServiceProvider.GetService(dbContextType) as DbContext;
            if (dbContext is null)
                continue;

            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Migration applied for {DbContext}", dbContextType.Name);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}