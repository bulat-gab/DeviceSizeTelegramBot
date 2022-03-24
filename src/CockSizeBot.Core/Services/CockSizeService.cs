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
    public async Task<int> GetSize(long userId, string? username = null)
    {
        try
        {
            DateTime today = DateTime.UtcNow.Date;

            if (this.cache.TryGetValue(userId, out int valueInCache))
            {
                this.logger.Information($"UserId: {userId} size found in cache: {valueInCache}");
                return valueInCache;
            }

            this.logger.Debug($"UserId: {userId}, Username: {username} cache miss");

            var measurement = await this.myDbContext.Measurements
            .Where(m => m.UserId == userId && m.Timestamp.Date == today)
            .FirstOrDefaultAsync();
            if (measurement != null)
            {
                this.logger.Information($"UserId: {userId}, Username: {username} size found in database: {measurement.CockSize}");
                return measurement.CockSize;
            }

            this.logger.Information($"{userId} database has no measurements for {today:dd-MM-yyyy}");

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
        this.myDbContext.SaveChanges();
    }

    public async Task EnsureUserExists(Telegram.Bot.Types.User telegramUser)
    {
        if (this.cache.TryGetValue(telegramUser.Id, out var _))
        {
            return;
        }

        this.logger.Information($"UserId: {telegramUser.Id}, Username: {telegramUser.Username} does not exist in cache");

        var user = new User
        {
            Id = telegramUser.Id,
            FirstName = telegramUser.FirstName,
            LastName = telegramUser.LastName,
            Username = telegramUser.Username,
        };

        using var transaction = this.myDbContext.Database.BeginTransaction();

        var userFromDb = await this.myDbContext.Users.FindAsync(user.Id);
        if (userFromDb != null)
            return;

        this.logger.Information($"UserId: {telegramUser.Id}, Username: {telegramUser.Username} does not exist in database");

        this.myDbContext.Users.Add(user);
        this.myDbContext.Database.ExecuteSqlRaw("Set IDENTITY_INSERT Users ON");

        this.myDbContext.SaveChanges();
        transaction.Commit();

        this.logger.Debug($"UserId: {telegramUser.Id}, Username: {telegramUser.Username} has been added into database");
    }
}
