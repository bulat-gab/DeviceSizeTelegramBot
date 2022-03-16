using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CockSizeBot.Core.TelegramUpdateHandlers;

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ILogger _logger = Log.ForContext<TextMessageHandler>();
    private readonly ITelegramBotClient _bot;

    public TextMessageHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task BotOnMessageReceived(Message message)
    {
        _logger.Information($"Received message type: {message.Type}, text: {message.Text}");
        if (message.Type != MessageType.Text || message.ViaBot != null)
            return;

        var action = message.Text!.Split(' ')[0] switch
        {
            "/help" => this.Usage(message),
            _ => this.Usage(message),
        };
        Message sentMessage = await action;
        _logger.Information($"The message was sent with id: {sentMessage.MessageId}");
    }

    private async Task<Message> Usage(Message message)
    {
        return await _bot.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: Constants.Usage,
            replyMarkup: new ReplyKeyboardRemove());
    }
}
