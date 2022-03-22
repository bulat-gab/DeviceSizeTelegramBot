using CockSizeBot.Domain;
using CockSizeBot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace CockSizeBot.Core.Services;

public class CockSizeService : ICockSizeService
{
    private readonly ILogger logger = Log.ForContext<CockSizeService>();
    private readonly ICockSizeGenerator cockSizeGenerator;
    private readonly IMemoryCache cache;
    private readonly MyDbContext myDbContext;

    public CockSizeService(
        ICockSizeGenerator cockSizeGenerator,
        IMemoryCache cache,
        MyDbContext dbContext)
    {
        this.cockSizeGenerator = cockSizeGenerator;
        this.cache = cache;
        this.myDbContext = dbContext;
    }

    /// <summary>
    /// Gets the cock size from the cache. Generates a new cock size if cannot find in the cache.
    /// </summary>
    /// <param name="userId">Telegram user id.</param>
    /// <returns>Integer value.</returns>
    public async Task<int> GetSize(long userId)
    {
        DateTime today = DateTime.UtcNow.Date;

        if (cache.TryGetValue(userId, out int valueInCache))
        {
            this.logger.Information($"UserId: {userId} size found in cache: {valueInCache}");
            return valueInCache;
        }

        this.logger.Debug($"{userId} cache miss");

        var measurement = await this.myDbContext.Measurements
            .Where(m => m.User.Id == userId && m.Timestamp.Date == today)
            .FirstOrDefaultAsync();
        if (measurement != null)
        {
            this.logger.Information($"UserId: {userId} size found in database: {measurement.CockSize}");
            return measurement.CockSize;
        }

        this.logger.Debug($"{userId} database has no measurements for {today:dd-MM-yyyy}");

        var size = this.cockSizeGenerator.Generate();
        await this.PersistNewValueAsync(userId, size);
        this.logger.Information($"UserId: {userId} new size: {size}");

        return size;
    }

    private async Task PersistNewValueAsync(long userId, int size)
    {
        this.cache.Set(userId, size, DateTimeOffset.UtcNow.AddSeconds(Constants.AbsoluteExpirationInSeconds));

        var measurement = new Measurement
        {
            CockSize = size,
            Timestamp = DateTime.UtcNow,
            User = new User
            {
                Id = userId,
            },
        };
        await this.myDbContext.Measurements.AddAsync(measurement);
    }
}
