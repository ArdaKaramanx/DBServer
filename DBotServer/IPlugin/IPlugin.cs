using Discord.WebSocket;
using Discord;
using System.Collections.Generic;
using System;

public interface IPlugin
{
    string Name { get; set; }

    void Initialize(DiscordSocketClient client);

    void Dispose(DiscordSocketClient client);

    List<SlashCommandProperties> GetSlashCommands();

    void OnCommandExecuted(SocketSlashCommand command);
}
