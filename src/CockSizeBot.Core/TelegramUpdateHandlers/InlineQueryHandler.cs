using CockSizeBot.Core.Services;
using Serilog;
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
        this.logger.Information($"Received inline query from: {userId}, username: {username}");

        string measurementResult = await this.Measure(userId, username);

        InlineQueryResult[] results =
            {
                    new InlineQueryResultArticle(
                        id: "3",
                        title: "Measure",
                        inputMessageContent:
                            new InputTextMessageContent(measurementResult)),
            };

        await this.bot.AnswerInlineQueryAsync(
        inlineQueryId: inlineQuery.Id,
        results: results,
        isPersonal: true,
        cacheTime: 0);
    }

    private async Task<string> Measure(long userId, string? username)
    {
        try
        {
            var cockSize = await this.cockSizeService.GetSize(userId);
            var emoji = this.emojiService.GetEmoji(cockSize);

            this.logger.Information($"{username} cock size is: {cockSize} {emoji}");

            return string.Format(Constants.MyMeasurement, cockSize, emoji);
        }
        catch (CockSizeBotException)
        {
            return Constants.BotIsNotAvailable;
        }
    }
}
