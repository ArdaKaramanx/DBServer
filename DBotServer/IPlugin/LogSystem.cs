using Discord;
using DiscordBotServer;

public static class LogSystem
{
    public static void Log(string message, Color colorCode)
    {
        ServerConsole.instance.AppendLog(message, colorCode);
    }
}

