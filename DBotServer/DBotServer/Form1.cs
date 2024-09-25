using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordBotServer
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

        public PluginLoader _pluginLoader;
        public DiscordSocketClient _client;

        private List<string> _commandHistory = new List<string>();
        private int _commandIndex = -1;

        private Timer _logTimer;
        private string _logDirectory;
        private bool _isBotRunning = false; // Botun çalışıp çalışmadığını takip eden bir bayrak

        public System.Drawing.Icon activeIcon = IconHelper.GetEmbeddedIcon("DiscordBotServer.ICO.ActiveMode.ico");
        public System.Drawing.Icon deactiveIcon = IconHelper.GetEmbeddedIcon("DiscordBotServer.ICO.DeactiveMode.ico");
        public System.Drawing.Icon normalIcon = IconHelper.GetEmbeddedIcon("DiscordBotServer.ICO.NormalMode.ico");

        public Form1()
        {
            instance = this;

            InitializeComponent();
            InitializeDiscordBot();
            InitializeLogDirectory();
            InitializeLogTimer();

            notifyIcon1.Icon = normalIcon;

            discordServerToolStripMenuItem.Text = "Server Is Stoped";
            discordServerToolStripMenuItem.ForeColor = Color.DarkRed;
        }

        public async void InitializeDiscordBot()
        {
            await StartBotAsync(); // Botu başlat
        }

        private async Task StartBotAsync()
        {
            if (_isBotRunning) return;

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged & ~GatewayIntents.GuildInvites // "GuildInvites" intent'ini kaldırıyoruz
            };

            _client = new DiscordSocketClient(config);
            _client.Log += Log;

            try
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                if (!File.Exists(configFilePath))
                {
                    AppendLog("Config file not found.", Color.Red);
                    return;
                }

                string configJson = File.ReadAllText(configFilePath);
                var botConfig = JsonConvert.DeserializeObject<BotConfig>(configJson);
                string token = botConfig.DiscordBotToken;

                if (string.IsNullOrEmpty(token))
                {
                    AppendLog("Bot token is not set.", Color.Red);
                    return;
                }

                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();

                _client.Ready += async () =>
                {
                    AppendLog("Bot is connected!", Color.Green);
                    try
                    {
                        discordServerToolStripMenuItem.Text = "Server Is Running";
                        discordServerToolStripMenuItem.ForeColor = Color.DarkGreen;
                        serverStartToolStripMenuItem.Text = "Server Stop";

                        notifyIcon1.Icon = activeIcon;

                        // Tüm global komutları sil
                        await _client.Rest.DeleteAllGlobalCommandsAsync();
                    }
                    catch (Exception ex)
                    {
                        notifyIcon1.Icon = deactiveIcon;
                        AppendLog("Error in Ready event: " + ex, Color.Red);
                    }
                };

                _isBotRunning = true;

                string pathToPlugins = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
                _pluginLoader = new PluginLoader(pathToPlugins);
            }
            catch (Exception ex)
            {
                AppendLog($"Error initializing bot: {ex.Message}", Color.Red);
            }
        }

        private async Task StopBotAsync()
        {
            if (!_isBotRunning) return;

            notifyIcon1.Icon = deactiveIcon;

            try
            {
                await _client.StopAsync(); // Botu durdur
                _isBotRunning = false;
                AppendLog("Bot stopped.", Color.Red);
                discordServerToolStripMenuItem.Text = "Server Is Stoped";
                discordServerToolStripMenuItem.ForeColor = Color.DarkRed;
                serverStartToolStripMenuItem.Text = "Server Start";
            }
            catch (Exception ex)
            {
                AppendLog($"Error stopping bot: {ex.Message}", Color.Red);
            }
        }

        private void InitializeLogDirectory()
        {
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Logs");

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        private void InitializeLogTimer()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (File.Exists(configFilePath))
            {
                string configJson = File.ReadAllText(configFilePath);
                var botConfig = JsonConvert.DeserializeObject<BotConfig>(configJson);

                if (botConfig != null && botConfig.LogIntervalSeconds > 0)
                {
                    _logTimer = new Timer
                    {
                        Interval = botConfig.LogIntervalSeconds * 1000 // Milisaniye cinsinden
                    };
                    _logTimer.Tick += LogTimer_Tick;
                    _logTimer.Start();
                }
            }
        }

        private void LogTimer_Tick(object sender, EventArgs e)
        {
            string logFileName = "[" + DateTime.Now.ToString("yyyy/MM/dd_HH.mm.ss") + "] - Console Log.txt";
            string logFilePath = Path.Combine(_logDirectory, logFileName);

            File.WriteAllText(logFilePath, richTextBoxConsole.Text);
        }

        public Task Log(LogMessage msg)
        {
            AppendLog(msg.ToString(), Color.Blue);
            return Task.CompletedTask;
        }

        public void AppendLog(string message, Color color)
        {
            if (richTextBoxConsole.InvokeRequired)
            {
                richTextBoxConsole.Invoke(new Action<string, Color>(AppendLog), new object[] { message, color });
            }
            else
            {
                string timestamp = "[" + DateTime.Now + "] ";
                richTextBoxConsole.SelectionStart = richTextBoxConsole.Text.Length;
                richTextBoxConsole.SelectionLength = 0;

                richTextBoxConsole.SelectionColor = Color.Gold;
                richTextBoxConsole.AppendText(timestamp);

                richTextBoxConsole.SelectionColor = color;
                richTextBoxConsole.AppendText(message + Environment.NewLine);

                richTextBoxConsole.SelectionStart = richTextBoxConsole.Text.Length;
                richTextBoxConsole.SelectionColor = richTextBoxConsole.ForeColor;

                richTextBoxConsole.SelectionStart = richTextBoxConsole.Text.Length;
                richTextBoxConsole.ScrollToCaret();
            }
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                var command = textBox1.Text.Trim().ToLower();

                if (command.StartsWith("plugin reload"))
                {
                    if (command.Contains("all"))
                    {
                        string pathToPlugins = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
                        _pluginLoader.UnloadPlugins();
                        _pluginLoader.LoadPlugins(pathToPlugins, _client);
                        AppendLog("Plugins Reloaded", Color.Green);
                    }
                    else
                    {
                        AppendLog("available codes: all", Color.Orange);
                    }
                }
                else if (command == "stop")
                {
                    await StopBotAsync(); // Botu durdur
                }
                else if (command == "start")
                {
                    await StartBotAsync(); // Botu başlat
                }
                else if (command.StartsWith("plugin stop"))
                {
                    if (command.Contains("all"))
                    {
                        _pluginLoader.UnloadPlugins();
                        AppendLog("All plugins stopped", Color.Green);
                    }
                    else
                    {
                        if (command.Length > "plugin stop ".Length)
                        {
                            var pluginName = command.Substring("plugin stop ".Length);
                            _pluginLoader.UnloadPluginByName(pluginName);
                        }
                        else
                        {
                            AppendLog("Error: Plugin name not provided. Please specify a plugin name after 'stop'.", Color.Red);
                            AppendLog("Usage: stop [Plugin Name]", Color.Green);
                        }
                    }
                }
                else if (command.StartsWith("plugin start"))
                {
                    if (command.Contains("all"))
                    {
                        string pathToPlugins = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
                        _pluginLoader.LoadPlugins(pathToPlugins, _client);
                        AppendLog("All plugins started", Color.Green);
                    }
                    else
                    {
                        if (command.Length > "plugin start ".Length)
                        {
                            var pluginName = command.Substring("plugin start ".Length);
                            _pluginLoader.LoadPluginByName(pluginName);
                        }
                        else
                        {
                            AppendLog("Error: Plugin name not provided. Please specify a plugin name after 'start'.", Color.Red);
                            AppendLog("Usage: start all or start [Plugin Name]", Color.Green);
                        }
                    }
                }
                else if (command == "plugin list")
                {
                    if (_pluginLoader._plugins.Count != 0)
                    {
                        foreach (IPlugin item in _pluginLoader._plugins)
                        {
                            AppendLog(item.Name, Color.DarkOrange);
                        }
                    }
                    else
                    {
                        AppendLog("Any plugin is not working", Color.DarkBlue);
                    }
                }
                else if (command == "clear")
                {
                    richTextBoxConsole.Text = string.Empty;
                }
                else if (command == "exit")
                {
                    Application.Exit();
                }
                else if (command == "help")
                {
                    AppendLog("All Command:\n" +
                        "plugin reload all = all plugins reload command\n" +
                        "plugin stop [Plugin Name / all] = Plugin stop command\n" +
                        "plugin start [Plugin Name / all] = Plugin start command\n" +
                        "plugin list = all plugin names print command\n" +
                        "start = bot start command\n" +
                        "stop = bot stop command\n" +
                        "clear = chat clear\n" +
                        "exit = Application quit command", Color.Green);
                }
                else
                {
                    AppendLog("This command does not exist", Color.Red);
                }

                if (!string.IsNullOrEmpty(command))
                {
                    _commandHistory.Add(command);
                    _commandIndex = -1;
                }

                textBox1.Text = string.Empty;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_commandHistory.Count > 0 && _commandIndex < _commandHistory.Count - 1)
                {
                    _commandIndex++;
                    textBox1.Text = _commandHistory[_commandHistory.Count - 1 - _commandIndex];
                    textBox1.SelectionStart = textBox1.Text.Length;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (_commandIndex > 0)
                {
                    _commandIndex--;
                    textBox1.Text = _commandHistory[_commandHistory.Count - 1 - _commandIndex];
                    textBox1.SelectionStart = textBox1.Text.Length;
                    e.SuppressKeyPress = true;
                }
                else if (_commandIndex == 0)
                {
                    _commandIndex = -1;
                    textBox1.Text = string.Empty;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide(); // Formu gizle
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private async void serverStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_isBotRunning)
            {
                await StartBotAsync();
            }
            else
            {
                await StopBotAsync();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Hide();
            }
        }
    }
}