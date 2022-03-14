using CockSizeBot.Core.TelegramUpdateHandlers;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CockSizeBot.Core;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger _logger = Log.ForContext<UpdateHandler>();
    private readonly IInlineQueryHandler _inlineQueryHandler;
    private readonly ITextMessageHandler _textMessageHandler;

    public UpdateHandler(
        IInlineQueryHandler inlineQueryHandler,
        ITextMessageHandler textMessageHandler)
    {
        _inlineQueryHandler = inlineQueryHandler;
        _textMessageHandler = textMessageHandler;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString(),
        };

        _logger.Error(errorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => _textMessageHandler.BotOnMessageReceived(update.Message!),
            UpdateType.InlineQuery => _inlineQueryHandler.BotOnInlineQueryReceived(update.InlineQuery!),
            _ => this.UnknownUpdateHandlerAsync(update),
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await this.HandleErrorAsync(bot, exception, cancellationToken);
        }
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.Warning($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }
}
