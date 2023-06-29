namespace NBP.EF.Models;

public class CurrencyRate
{
    public int Id { get; }
    public string Table { get; private set; }
    public string CurrencyCode { get; private set; }
    public string Currency { get; private set; }
    public DateOnly Date { get; private set; }
    public decimal Rate { get; private set; }

    private CurrencyRate()
    {
        // ef constructor 
    }

    public CurrencyRate(string currencyCode, string currency, DateOnly date, string table)
    {
        CurrencyCode = currencyCode;
        Currency = currency;
        Date = date;
        Table = table;
    }

    public void SetRate(decimal rate)
    {
        Rate = rate;
    }
}