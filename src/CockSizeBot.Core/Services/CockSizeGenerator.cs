namespace CockSizeBot.Core.Services;

public class CockSizeGenerator : ICockSizeGenerator
{
    private const int MinSize = 1;
    private const int MaxSizeExcluded = 51;

    private readonly Random _random;

    public CockSizeGenerator()
    {
        _random = new Random();
    }

    public int Generate()
    {
        return _random.Next(MinSize, MaxSizeExcluded);
    }
}
