using Serilog;
using Serilog.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CockSizeBot.Core;
public static class ApplicationInitializer
{
    public static Logger InitilalizeSerilog()
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($"logs\\log-{DateTime.UtcNow.ToString("dd/MM/yyyy")}.log",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Information("Serilog initialized");
        return logger;
    }

    public static async Task<ITelegramBotClient> InitializeTelegramBot()
    {
        string? token = Environment.GetEnvironmentVariable(Constants.TelegramBotToken)
            ?? Environment.GetEnvironmentVariable(Constants.TelegramBotToken, EnvironmentVariableTarget.Machine);
        if (token == null)
        {
            Log.Error("Could not fetch Telegram Bot Token from Environment Variables");
            throw new ArgumentNullException(nameof(token));
        }

        var bot = new TelegramBotClient(token);

        User me = await bot.GetMeAsync();
        Log.Information($"Start listening for @{me.Username}");

        return bot;
    }
}
