using Discord;
using DiscordBotServer;

public static class LogSystem
{
    public static void Log(string message, Color colorCode)
    {
        Form1.instance.AppendLog(message, colorCode);
    }
}

