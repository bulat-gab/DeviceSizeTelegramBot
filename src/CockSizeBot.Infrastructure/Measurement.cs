namespace CockSizeBot.Infrastructure;

public class Measurement
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public int CockSize { get; set; }
}
