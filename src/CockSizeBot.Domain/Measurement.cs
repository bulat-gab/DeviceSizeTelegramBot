namespace CockSizeBot.Domain;

public class Measurement
{
    public long Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int CockSize { get; set; }

    public long UserId { get; set; }

    public Measurement() { }

    public Measurement(int cockSize, long userId)
    {
        this.Timestamp = DateTime.UtcNow;
        this.CockSize = cockSize;
        this.UserId = userId;
    }
}
