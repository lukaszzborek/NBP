namespace NBP.Clients.Exceptions;

public class InvalidNBPTableException : Exception
{
    public string Table { get; }

    public InvalidNBPTableException(string table) : base($"Invalid NBP table: {table}")
    {
        Table = table;
    }
}