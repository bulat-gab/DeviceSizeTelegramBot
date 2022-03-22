namespace CockSizeBot.Domain;

public class Measurement
{
    public long Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int CockSize { get; set; }

    public User User { get; set; }
}
