using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotServer
{
    public class BotConfig
    {
        public string DiscordBotToken { get; set; }
        public int LogIntervalSeconds { get; set; } // Log aralık süresi (saniye)
    }
}
