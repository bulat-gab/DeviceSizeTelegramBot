namespace CockSizeBot.Core.Services;

public class EmojiService : IEmojiService
{
    public string GetEmoji(int cockSize)
    {
        if (cockSize < 5)
            return "😭";
        if (cockSize < 10)
            return "🙁";
        if (cockSize >= 10 && cockSize < 15)
            return "😐";
        if (cockSize >= 15 && cockSize < 20)
            return "😏";
        if (cockSize >= 20 && cockSize < 25)
            return "😮";
        if (cockSize >= 25 && cockSize < 30)
            return "🥳";
        if (cockSize >= 30 && cockSize < 35)
            return "😨";
        if (cockSize >= 35 && cockSize < 40)
            return "😱";
        if (cockSize >= 40 && cockSize < 45)
            return "🤪";
        if (cockSize >= 45 && cockSize < 48)
            return "😎";
        if (cockSize == 48)
            return "🔥";
        if (cockSize == 49)
            return "🔥🔥";
        if (cockSize == 50)
            return "🔥🔥🔥";

        return string.Empty;
    }
}
