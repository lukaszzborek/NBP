namespace NBP.Kernel;

public class Clock : IClock
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}