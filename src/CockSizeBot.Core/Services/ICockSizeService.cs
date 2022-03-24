namespace CockSizeBot.Core.Services;

public interface ICockSizeService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="username"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the size.</returns>
    /// <exception cref="CockSizeBotException">Throws exception if the cache or database is not reachable</exception>
    public Task<int> GetSize(long userId, string? username = null);

    public Task EnsureUserExists(Telegram.Bot.Types.User user);
}
