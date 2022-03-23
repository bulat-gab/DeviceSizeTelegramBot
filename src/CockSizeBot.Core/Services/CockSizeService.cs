using CockSizeBot.Core.Helpers;
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
        try
        {
            DateTime today = DateTime.UtcNow.Date;

            if (this.cache.TryGetValue(userId, out int valueInCache))
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

            this.AddNewValueToCache(userId, size);
            await this.AddNewValueToDatabase(userId, size);

            this.logger.Information($"UserId: {userId} new size: {size}");

            return size;
        }
        catch (Exception exception)
        {
            var errorMessage = $"{nameof(CockSizeService)} failed.";
            this.logger.Error(exception, errorMessage);
            throw new CockSizeBotException(errorMessage, exception);
        }
    }

    private void AddNewValueToCache(long userId, int size)
    {
        var nextResetTimestamp = DateTimeHelper.GetNextResetTime();
        this.cache.Set(userId, size, nextResetTimestamp);
    }

    private async Task AddNewValueToDatabase(long userId, int size)
    {
        var measurement = new Measurement(size, userId);
        await this.myDbContext.Measurements.AddAsync(measurement);
    }
}
