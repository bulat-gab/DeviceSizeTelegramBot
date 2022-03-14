using Telegram.Bot.Types;

namespace CockSizeBot.Core.TelegramUpdateHandlers;

public interface IInlineQueryHandler
{
    Task BotOnInlineQueryReceived(InlineQuery inlineQuery);
}
