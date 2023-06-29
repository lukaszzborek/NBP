using MediatR;
using Microsoft.EntityFrameworkCore;
using NBP.EF;
using NBP.EF.Models;

namespace NBP.Queries;

public record GetCurrencyRatesByDate : IRequest<List<CurrencyRate>>;

public class GetCurrencyRatesByDateHandler : IRequestHandler<GetCurrencyRatesByDate, List<CurrencyRate>>
{
    private readonly CurrencyDbContext _currencyDbContext;

    public GetCurrencyRatesByDateHandler(CurrencyDbContext currencyDbContext)
    {
        _currencyDbContext = currencyDbContext;
    }

    // N+1 problem
    // needs some optimization
    // maybe use raw query with over PARTITION  by
    public async Task<List<CurrencyRate>> Handle(GetCurrencyRatesByDate request, CancellationToken cancellationToken)
    {
        var maxDatesPerTable = await _currencyDbContext.CurrencyRates
                                                       .GroupBy(cr => cr.Table)
                                                       .Select(g => new { Table = g.Key, MaxDate = g.Max(x => x.Date) })
                                                       .ToListAsync(cancellationToken);

        var result = new List<CurrencyRate>();
        foreach (var item in maxDatesPerTable)
        {
            var currencyRates = await _currencyDbContext.CurrencyRates
                                                        .Where(cr => cr.Table == item.Table && cr.Date == item.MaxDate)
                                                        .ToListAsync(cancellationToken);
            result.AddRange(currencyRates);
        }

        return result.OrderBy(x => x.CurrencyCode).ToList();
    }
}