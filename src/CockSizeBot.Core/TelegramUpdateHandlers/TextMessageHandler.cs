using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CockSizeBot.Core.TelegramUpdateHandlers;

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ILogger logger = Log.ForContext<TextMessageHandler>();
    private readonly ITelegramBotClient bot;

    public TextMessageHandler(ITelegramBotClient bot) => this.bot = bot;

    public async Task BotOnMessageReceived(Message message)
    {
        if (message.Type != MessageType.Text || message.ViaBot != null)
            return;

        logger.Information($"Received message: {message.Text}");

        var action = message.Text!.Split(' ')[0] switch
        {
            "/help" => this.Usage(message),
            _ => this.Usage(message),
        };
        Message sentMessage = await action;
        logger.Information($"The message was sent with id: {sentMessage.MessageId}");
    }

    private async Task<Message> Usage(Message message) => await this.bot.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: Constants.Usage,
            replyMarkup: new ReplyKeyboardRemove());
}
