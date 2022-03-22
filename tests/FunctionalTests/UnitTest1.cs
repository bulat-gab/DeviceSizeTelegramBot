using CockSizeBot.Core.Services;
using CockSizeBot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace FunctionalTests;

public class Tests
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

        this.cocksizeService = new CockSizeService(this.cockSizeGenerator, this.cache);
    }

    [Test]
    public void GetSize_ShouldReturnValueFromCache()
    {
        long userId = 1;
        int expectedSize = 15;
        this.cache.Set(userId, expectedSize);

        var actual = this.cocksizeService.GetSize(userId);

        Assert.AreEqual(expectedSize, actual);
    }

    [Test]
    public void GetSize_ShouldReturnNewValue()
    {
        long userId = 1;

        var actual = this.cocksizeService.GetSize(userId);

        Assert.AreEqual(Size, actual);
    }
}
