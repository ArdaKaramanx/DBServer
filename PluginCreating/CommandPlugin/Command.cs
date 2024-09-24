using Discord;
using Discord.WebSocket;
using System.Collections.Generic;

namespace CommandPlugin
{
    public class Command : IPlugin
    {
        private DiscordSocketClient _client;

        // Plugin'in adı
        public string Name { get; set; } = "commandplugin.dll";

        // Bot başlatıldığında gerekli ayarları yapar
        public void Initialize(DiscordSocketClient client)
        {
            _client = client;
            LogSystem.Log("Ready Command Test Plugin", Color.Green);
        }

        // Bot durdurulduğunda yapılan işlemler
        public void Dispose(DiscordSocketClient client)
        {
            LogSystem.Log("UnReady Command Test Plugin", Color.Red);
        }

        // Slash komutlarını tanımla ve döndür
        List<SlashCommandProperties> IPlugin.GetSlashCommands()
        {
            var commands = new List<SlashCommandProperties>();
            var pingCommand = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Responds with Pong!")
                .Build();

            commands.Add(pingCommand);
            return commands;
        }

        // Slash komut işlendiğinde çağrılır
        public async void OnCommandExecuted(SocketSlashCommand command)
        {
            if (command.Data.Name == "ping")
            {
                await command.RespondAsync("Pong!");
            }
        }
    }
}
