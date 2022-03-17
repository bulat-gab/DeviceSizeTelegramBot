namespace CockSizeBot.Core.Services;

public class CockSizeGenerator : ICockSizeGenerator
{
    private const int MinSize = 1;
    private const int MaxSizeExcluded = 51;

    private readonly Random random;

    public CockSizeGenerator() => this.random = new Random();

    public int Generate() => this.random.Next(MinSize, MaxSizeExcluded);
}
