using CockSizeBot.Core.Services;
using Serilog;
using Serilog.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CockSizeBot.Core.TelegramUpdateHandlers;

public class InlineQueryHandler : IInlineQueryHandler
{
    private readonly ILogger logger = Log.ForContext<InlineQueryHandler>();
    private readonly ICockSizeService cockSizeService;
    private readonly ITelegramBotClient bot;
    private readonly IEmojiService emojiService;

    public InlineQueryHandler(ITelegramBotClient bot, ICockSizeService cockSizeService, IEmojiService emodjiService)
    {
        this.cockSizeService = cockSizeService;
        this.bot = bot;
        this.emojiService = emodjiService;
    }

    public async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
    {
        long userId = inlineQuery.From.Id;
        string? username = inlineQuery.From.Username;

        using (LogContext.PushProperty("userId", userId))
        using (LogContext.PushProperty("username", username))
        {
            this.logger.Information($"Received inline query from: {userId}, username: {username}");
            var cockSize = await this.cockSizeService.GetSize(userId);
            var emoji = emojiService.GetEmoji(cockSize);
            this.logger.Information($"{username} cock size is: {cockSize} {emoji}");

            InlineQueryResult[] results =
            {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "Measure",
                    inputMessageContent:
                        new InputTextMessageContent(string.Format(Constants.MyMeasurement, cockSize, emoji))),
            };

            await this.bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: Constants.AbsoluteExpirationInSeconds);
        }
    }
}
