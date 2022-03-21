using CockSizeBot.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

Log.Logger = ApplicationInitializer.InitilalizeSerilog();
var bot = await ApplicationInitializer.InitializeTelegramBot();

var serviceProvider = DependencyInjection
    .GetServices()
    .AddTelegramBot(bot)
    .Build();

var handler = serviceProvider.GetService<IUpdateHandler>();
ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
bot.StartReceiving(
    handler.HandleUpdateAsync,
    handler.HandleErrorAsync,
    receiverOptions);

while (true)
{
    Thread.Sleep(100);
}
