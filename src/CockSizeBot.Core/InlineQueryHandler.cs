using CockSizeBot.Core.Services;
using CockSizeBot.Core.TelegramUpdateHandlers;
using Serilog;
using Serilog.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CockSizeBot.Core;

public class InlineQueryHandler : IInlineQueryHandler
{
    private readonly ILogger _logger = Log.ForContext<InlineQueryHandler>();
    private readonly ICockSizeService _cockSizeService;
    private readonly ITelegramBotClient _bot;
    private readonly IEmojiService _emojiService;

    public InlineQueryHandler(ITelegramBotClient bot, ICockSizeService cockSizeService, IEmojiService emodjiService)
    {
        _cockSizeService = cockSizeService;
        _bot = bot;
        _emojiService = emodjiService;
    }

    public async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
    {
        long userId = inlineQuery.From.Id;
        string? username = inlineQuery.From.Username;

        using (LogContext.PushProperty("userId", userId))
        using (LogContext.PushProperty("username", username))
        {
            _logger.Information($"Received inline query from: {userId}, username: {username}");
            var cockSize = _cockSizeService.GetSize(userId);
            var emoji = _emojiService.GetEmoji(cockSize);
            _logger.Information($"{username} cock size is: {cockSize} {emoji}");

            InlineQueryResult[] results =
            {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "Measure",
                    inputMessageContent:
                        new InputTextMessageContent(string.Format(Constants.MyMeasurement, cockSize, emoji))),
            };

            await _bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 3600);
        }
    }
}
