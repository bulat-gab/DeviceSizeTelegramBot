namespace CockSizeBot.Core.Services;

public interface ICockSizeService
{
    public Task<int> GetSize(long userId);
}
