using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace CockSizeBot.Core.Services;

public class CockSizeService : ICockSizeService
{
    private readonly ILogger _logger = Log.ForContext<CockSizeService>();
    private ICockSizeGenerator _cockSizeGenerator;
    private readonly IMemoryCache _cache;

    public CockSizeService(ICockSizeGenerator cockSizeGenerator, IMemoryCache cache)
    {
        _cockSizeGenerator = cockSizeGenerator;
        _cache = cache;
    }

    /// <summary>
    /// Gets the cock size from the cache. Generates a new cock size if cannot find in the cache.
    /// </summary>
    /// <param name="userId">Telegram user id.</param>
    /// <returns>Integer value.</returns>
    public int GetSize(long userId)
    {
        if (!_cache.TryGetValue(userId, out int cockSize))
        {
            _logger.Debug($"{userId} cache miss");
            cockSize = _cockSizeGenerator.Generate();
            _cache.Set(userId, cockSize, DateTimeOffset.UtcNow.AddHours(24));
        }

        return cockSize;
    }
}
