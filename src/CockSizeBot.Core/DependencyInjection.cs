using CockSizeBot.Core.Services;
using CockSizeBot.Core.TelegramUpdateHandlers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace CockSizeBot.Core;

public static class DependencyInjection
{
    public static IServiceCollection GetServices() => new ServiceCollection()
            .AddMemoryCache()
            .AddSingleton<ICockSizeGenerator, CockSizeGenerator>()
            .AddSingleton<ICockSizeService, CockSizeService>()
            .AddSingleton<IUpdateHandler, UpdateHandler>()
            .AddSingleton<IInlineQueryHandler, InlineQueryHandler>()
            .AddSingleton<ITextMessageHandler, TextMessageHandler>()
            .AddSingleton<IEmojiService, EmojiService>()
            ;

    public static IServiceCollection AddTelegramBot(this IServiceCollection services, ITelegramBotClient bot) => services.AddSingleton(bot);

    public static IServiceProvider Build(this IServiceCollection services) => services.BuildServiceProvider();
}
