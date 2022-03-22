using System;
using System.Threading.Tasks;
using CockSizeBot.Core.Services;
using CockSizeBot.Domain;
using CockSizeBot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace FunctionalTests;

public class CockSizeServiceTest
{
    private const int Size = 42;

    private MyDbContext myDbContext;
    private CockSizeService cocksizeService;
    private ICockSizeGenerator cockSizeGenerator;
    private MemoryCache cache;

    [SetUp]
    public void Setup()
    {
        var dbOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        this.myDbContext = new MyDbContext(dbOptions);

        var mock = new Mock<ICockSizeGenerator>();
        mock.Setup(f => f.Generate()).Returns(Size);
        this.cockSizeGenerator = mock.Object;

        this.cache = new MemoryCache(new MemoryCacheOptions());

        this.cocksizeService = new CockSizeService(this.cockSizeGenerator, this.cache, this.myDbContext);
    }

    [Test]
    public async Task GetSize_ShouldReturnValueFromCacheAsync()
    {
        long userId = 1;
        int expectedSize = 15;
        this.cache.Set(userId, expectedSize);

        var actual = await this.cocksizeService.GetSize(userId);

        Assert.AreEqual(expectedSize, actual);
    }

    [Test]
    public async Task GetSize_ShouldReturnNewValueAsync()
    {
        long userId = 1;

        var actual = await this.cocksizeService.GetSize(userId);

        Assert.AreEqual(Size, actual);
    }

    [Test]
    public async Task GetSize_ShouldReturnValueFromDatabase()
    {
        var user = new User
        {
            Id = 1,
            Username = "Mark123",
            FirstName = "Mark",
        };

        var measurement = new Measurement
        {
            Id = 1,
            Timestamp = DateTime.UtcNow,
            CockSize = 5,
            User = user,
        };
        this.myDbContext.Users.Add(user);
        this.myDbContext.Measurements.Add(measurement);
        this.myDbContext.SaveChanges();

        var actual = await this.cocksizeService.GetSize(user.Id);

        Assert.AreEqual(measurement.CockSize, actual);
    }

    [Test]
    public async Task GetSize_ShouldReturnTodaysValueFromDatabase()
    {
        var user = new User
        {
            Id = 1,
            Username = "Mark123",
            FirstName = "Mark",
        };

        var oldMeasurement = new Measurement
        {
            Id = 1,
            Timestamp = DateTime.UtcNow.AddDays(-1),
            CockSize = 11,
            User = user,
        };
        var newMeasurement = new Measurement
        {
            Id = 2,
            Timestamp = DateTime.UtcNow,
            CockSize = 12,
            User = user,
        };

        this.myDbContext.Users.Add(user);
        this.myDbContext.Measurements.Add(oldMeasurement);
        this.myDbContext.Measurements.Add(newMeasurement);
        this.myDbContext.SaveChanges();

        var actual = await this.cocksizeService.GetSize(user.Id);

        Assert.AreEqual(newMeasurement.CockSize, actual);
    }
}
