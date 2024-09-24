using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace CommandPlugin
{
    public class AdmiralSunk : IPlugin
    {
        private DiscordSocketClient _client;
        private Dictionary<ulong, Game> _games = new Dictionary<ulong, Game>();

        string IPlugin.Name { get; set; } = "admiralsunk";

        public void Initialize(DiscordSocketClient client)
        {
            LogSystem.Log("AdmiralSunk Initialize", Color.DarkGreen);
            _client = client;
            _client.Ready += OnReady;
            _client.InteractionCreated += OnInteractionCreated;
        }

        public void Dispose(DiscordSocketClient client)
        {
            LogSystem.Log("AdmiralSunk Dispose", Color.DarkRed);
            _client.Ready -= OnReady;
            _client.InteractionCreated -= OnInteractionCreated;
        }

        private async Task OnReady()
        {
            var command = new SlashCommandBuilder()
                .WithName("admiralsunk")
                .WithDescription("Amiral Battı oyunu oynamak")
                .AddOption("action", ApplicationCommandOptionType.String, "Oyun eylemi (place/attack)", isRequired: true)
                .AddOption("x", ApplicationCommandOptionType.Integer, "X koordinatı", isRequired: false)
                .AddOption("y", ApplicationCommandOptionType.Integer, "Y koordinatı", isRequired: false);

            try
            {
                await _client.CreateGlobalApplicationCommandAsync(command.Build());
            }
            catch (Exception ex)
            {
                LogSystem.Log($"Error registering command: {ex.Message}", Color.Red);
            }
        }

        private async Task OnInteractionCreated(SocketInteraction interaction)
        {
            if (interaction is SocketSlashCommand slashCommand)
            {
                var userId = interaction.User.Id;

                if (!_games.ContainsKey(userId))
                {
                    _games[userId] = new Game(); // Yeni bir oyun başlatıyoruz
                }

                var game = _games[userId];
                var commandName = slashCommand.Data.Name;

                if (commandName == "admiralsunk")
                {
                    var actionOption = slashCommand.Data.Options.FirstOrDefault(o => o.Name == "action");
                    var xOption = slashCommand.Data.Options.FirstOrDefault(o => o.Name == "x");
                    var yOption = slashCommand.Data.Options.FirstOrDefault(o => o.Name == "y");

                    if (actionOption != null)
                    {
                        var action = actionOption.Value.ToString();

                        if (action == "place" && xOption != null && yOption != null)
                        {
                            var x = Convert.ToInt32(xOption.Value);
                            var y = Convert.ToInt32(yOption.Value);
                            game.PlaceShip(x, y);
                            game.IsInitialized = true; // Oyunu başlatmış olduk
                            await slashCommand.RespondAsync($"Gemi ({x}, {y}) konumuna yerleştirildi.");
                            await DisplayBoard(userId, slashCommand.Channel);
                        }
                        else if (action == "attack" && xOption != null && yOption != null)
                        {
                            var x = Convert.ToInt32(xOption.Value);
                            var y = Convert.ToInt32(yOption.Value);
                            var hit = game.Attack(x, y);
                            await slashCommand.RespondAsync(hit ? "Vurdu!" : "Iskaldı!");
                            await DisplayBoard(userId, slashCommand.Channel);
                            game.AiMove(); // Yapay zekanın hamlesi
                        }
                        else
                        {
                            await slashCommand.RespondAsync("Geçersiz komut veya koordinatlar.");
                        }
                    }
                    else
                    {
                        await slashCommand.RespondAsync("Eylem belirtilmedi. Lütfen `place` veya `attack` belirtin.");
                    }
                }
            }
        }

        private string GetBoardDisplay(ulong userId)
        {
            var game = _games[userId];
            var board = game.GetBoard();

            var boardMessage = "";
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var cell = board[x, y];
                    string cellEmoji;

                    switch (cell)
                    {
                        case CellType.Water:
                            cellEmoji = "🌊";
                            break;
                        case CellType.Ship:
                            cellEmoji = "🚢";
                            break;
                        case CellType.Hit:
                            cellEmoji = "💥";
                            break;
                        case CellType.Miss:
                            cellEmoji = "❌";
                            break;
                        case CellType.Empty:
                            cellEmoji = "⬜";
                            break;
                        default:
                            cellEmoji = "⬛"; // Bilinmeyen hücre tipi
                            break;
                    }

                    boardMessage += cellEmoji;
                }
                boardMessage += "\n";
            }

            return boardMessage;
        }

        private async Task DisplayBoard(ulong userId, IMessageChannel channel)
        {
            var boardMessage = GetBoardDisplay(userId);
            await channel.SendMessageAsync($"```markdown\n{boardMessage}```");
        }
    }

    public class Game
    {
        public CellType[,] PlayerBoard { get; private set; }
        public CellType[,] AiBoard { get; private set; }
        public bool IsInitialized { get; set; } = false;

        public Game()
        {
            PlayerBoard = new CellType[10, 10];
            AiBoard = new CellType[10, 10];
            InitializeBoard(PlayerBoard);
            InitializeBoard(AiBoard);
        }

        private void InitializeBoard(CellType[,] board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = CellType.Water;
                }
            }
        }

        public CellType[,] GetBoard()
        {
            return PlayerBoard; // Burada hangi tahtanın gösterileceğine karar verin.
        }

        public void PlaceShip(int x, int y)
        {
            if (x >= 0 && x < 10 && y >= 0 && y < 10)
            {
                PlayerBoard[x, y] = CellType.Ship;
            }
        }

        public bool Attack(int x, int y)
        {
            if (x >= 0 && x < 10 && y >= 0 && y < 10)
            {
                if (AiBoard[x, y] == CellType.Ship)
                {
                    AiBoard[x, y] = CellType.Hit;
                    return true;
                }
                else
                {
                    AiBoard[x, y] = CellType.Miss;
                }
            }
            return false;
        }

        public void AiMove()
        {
            Random rand = new Random();
            int x = rand.Next(0, 10);
            int y = rand.Next(0, 10);

            if (PlayerBoard[x, y] == CellType.Ship)
            {
                PlayerBoard[x, y] = CellType.Hit; // Vurdu
            }
            else
            {
                PlayerBoard[x, y] = CellType.Miss;
            }
        }
    }

    public enum CellType
    {
        Water,  // Su
        Ship,   // Gemi
        Hit,    // Vurulan nokta
        Miss,   // Iskaladık
        Empty   // Boş hücre
    }
}
