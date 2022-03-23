namespace CockSizeBot.Core;

public class CockSizeBotException : Exception
{
    public CockSizeBotException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
