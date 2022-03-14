using Telegram.Bot.Types;

namespace CockSizeBot.Core.TelegramUpdateHandlers;

public interface ITextMessageHandler
{
    Task BotOnMessageReceived(Message message);
}
