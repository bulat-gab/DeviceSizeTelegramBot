namespace CockSizeBot.Core;

public static class Constants
{
    public const string MyMeasurement = "My cock size is: {0}cm {1}";
    public const string YourMeasurement = "Your cock size is: {0}cm";
    public const string Usage = "Usage:\n" +
                                "Type @devicesizebot and click Measure button in order to measure your 'device' size\n";

    /// <summary>
    /// Expiration policy for the cache.
    /// </summary>
    public const int AbsoluteExpirationInSeconds = 3600 * 24;

    public const string TelegramBotToken = "TelegramBotToken";

    public const string BotIsNotAvailable = "The bot is not available at the moment.Please try again later.";
}
