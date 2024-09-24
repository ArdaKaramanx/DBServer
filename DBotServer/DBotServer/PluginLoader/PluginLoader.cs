using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Discord;
using DBotServer;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Rest;

public class PluginLoader
{
    public List<IPlugin> _plugins = new List<IPlugin>();
    public string _pluginsPath;

    public PluginLoader(string pluginsPath)
    {
        _pluginsPath = pluginsPath;
        LoadPlugins(pluginsPath, Form1.instance._client);
        Form1.instance._client.SlashCommandExecuted += HandleCommandExecuted;
    }

    public void LoadPlugins(string path, DiscordSocketClient client)
    {
        foreach (var file in Directory.GetFiles(path, "*.dll"))
        {
            try
            {
                byte[] assemblyData = File.ReadAllBytes(file);
                Assembly assembly = Assembly.Load(assemblyData);

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var plugin = (IPlugin)Activator.CreateInstance(type);
                        plugin.Initialize(client);
                        RegisterSlashCommands(plugin.GetSlashCommands(), client);
                        _plugins.Add(plugin);
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.instance.AppendLog($"Error loading plugin from {file}: {ex.Message}", Color.Red);
            }
        }
    }


    public void UnloadPlugins()
    {
        foreach (var plugin in _plugins.ToList())
        {
            try
            {
                plugin.Dispose(Form1.instance._client);
            }
            catch (Exception ex)
            {
                Form1.instance.AppendLog($"Error disposing plugin: {ex.Message}", Color.Red);
            }
        }
        _plugins.Clear();
    }

    public void LoadPluginByName(string pluginName)
    {
        string filePath = Path.Combine(_pluginsPath, $"{pluginName}.dll");

        if (File.Exists(filePath))
        {
            try
            {
                byte[] assemblyData = File.ReadAllBytes(filePath);
                Assembly assembly = Assembly.Load(assemblyData);

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var plugin = (IPlugin)Activator.CreateInstance(type);
                        plugin.Initialize(Form1.instance._client);
                        _plugins.Add(plugin);
                    }
                }
                Form1.instance.AppendLog($"Plugin {pluginName} started successfully.", Color.Green);
            }
            catch (Exception ex)
            {
                Form1.instance.AppendLog($"Error starting plugin {pluginName}: {ex.Message}", Color.Red);
            }
        }
        else
        {
            Form1.instance.AppendLog($"Plugin {pluginName}.dll not found in {_pluginsPath}", Color.Red);
        }
    }

    public void UnloadPluginByName(string pluginName)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Name == pluginName);

        if (plugin != null)
        {
            try
            {
                plugin.Dispose(Form1.instance._client);
                _plugins.Remove(plugin);
                Form1.instance.AppendLog($"Plugin {pluginName} stopped successfully.", Color.Green);
            }
            catch (Exception ex)
            {
                Form1.instance.AppendLog($"Error stopping plugin {pluginName}: {ex.Message}", Color.Red);
            }
        }
        else
        {
            Form1.instance.AppendLog($"Plugin {pluginName} is not currently loaded.", Color.Red);
        }
    }

    private async void RegisterSlashCommands(List<SlashCommandProperties> returnCommands, DiscordSocketClient client)
    {
        await Task.Delay(4000);
        Form1.instance.AppendLog($"Registering Commands", Color.DarkPurple);
        await Task.Delay(1000);

        try
        {
            int i = 0;
            foreach (var command in returnCommands)
            {
                i++;
                Form1.instance.AppendLog($"[{i}/{returnCommands.Count}] Command Registered: {command.Name}", Color.Purple);
                await client.CreateGlobalApplicationCommandAsync(command);
            }
        }
        catch (Exception ex)
        {
            Form1.instance.AppendLog($"Error registering global slash commands: {ex.Message}", Color.Red);
        }
    }

    private async Task HandleCommandExecuted(SocketSlashCommand command)
    {
        // Tüm pluginler arasında dön ve komutun hangi plugin tarafından tanımlandığını kontrol et
        foreach (var plugin in _plugins)
        {
            var runCommands = plugin.GetSlashCommands();
            // Eğer komut ismi, bu plugin tarafından tanımlanan komutlar arasındaysa, ilgili metodunu çağır
            if (runCommands.Any(c => c.Name.IsSpecified && c.Name.Value == command.Data.Name))
            {
                await Task.Run(() => plugin.OnCommandExecuted(command));
                // Komut işlendikten sonra döngüden çıkabilirsiniz.
                break;
            }
        }
    }

}