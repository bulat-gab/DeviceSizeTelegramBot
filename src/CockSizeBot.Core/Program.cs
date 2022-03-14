using Serilog;
using Telegram.Bot;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($"logs\\log-{DateTime.UtcNow.ToString("dd/MM/yyyy")}.log",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

Log.Information("Serilog initialized");

var token = Environment.GetEnvironmentVariable("TelegramBotToken", EnvironmentVariableTarget.Machine);
if (token == null)
{
    Log.Error("Could not fetch Telegram Bot Token from Environment Variables");
    return;
}

var botClient = new TelegramBotClient(token);

var me = await botClient.GetMeAsync();
Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
