using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace CockSizeBot.Core.Services;

public class CockSizeService : ICockSizeService
{
    private readonly ILogger logger = Log.ForContext<CockSizeService>();
    private readonly ICockSizeGenerator cockSizeGenerator;
    private readonly IMemoryCache cache;

    public CockSizeService(ICockSizeGenerator cockSizeGenerator, IMemoryCache cache)
    {
        this.cockSizeGenerator = cockSizeGenerator;
        this.cache = cache;
    }

    /// <summary>
    /// Gets the cock size from the cache. Generates a new cock size if cannot find in the cache.
    /// </summary>
    /// <param name="userId">Telegram user id.</param>
    /// <returns>Integer value.</returns>
    public int GetSize(long userId)
    {
        if (!cache.TryGetValue(userId, out int cockSize))
        {
            this.logger.Debug($"{userId} cache miss");
            cockSize = this.cockSizeGenerator.Generate();
            cache.Set(userId, cockSize, DateTimeOffset.UtcNow.AddHours(24));
        }

        return cockSize;
    }
}
