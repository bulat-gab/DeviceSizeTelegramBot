using CockSizeBot.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($"logs\\log-{DateTime.UtcNow.ToString("dd/MM/yyyy")}.log",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

Log.Information("Serilog initialized");

var token = Environment.GetEnvironmentVariable(Constants.TelegramBotToken)
    ?? Environment.GetEnvironmentVariable(Constants.TelegramBotToken, EnvironmentVariableTarget.Machine);
if (token == null)
{
    Log.Error("Could not fetch Telegram Bot Token from Environment Variables");
    return;
}

var bot = new TelegramBotClient(token);

User me = await bot.GetMeAsync();
Log.Information($"Start listening for @{me.Username}");

var serviceProvider = DependencyInjection
    .GetServices()
    .AddTelegramBot(bot)
    .Build();

var handler = serviceProvider.GetService<IUpdateHandler>();

using var cts = new CancellationTokenSource();
ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };

bot.StartReceiving(
    handler.HandleUpdateAsync,
    handler.HandleErrorAsync,
    receiverOptions,
    cts.Token);

while (true)
{
    Thread.Sleep(100);
}

// Send cancellation request to stop bot
cts.Cancel();
