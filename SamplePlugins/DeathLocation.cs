using fNbt;
using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
using MCServerWrapper.Utilities;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePlugins
{
    public class DeathLocation : IPlugin
    {
        public string Name => "Death Location";
        public string Description => "Says the location where somebody dies in chat";
        public void OnStart(IServerConsole console) { }
        public void OnExit(IServerConsole console) { }
        public void OnChatMessage(IServerConsole console, ServerChatMessage message) { }
        public void OnErrorMessage(IServerConsole console, ServerErrorMessage message) { }
        public void OnSuccessMessage(IServerConsole console, ServerSuccessMessage message) { }

        public async void OnOtherMessage(IServerConsole console, ServerMessage message)
        {
            string deathMessage = MatchesDeathMessage(message.Text);
            if (deathMessage == null) return;
            console.SendCommand("save-all", false, false);
            await Task.Delay(3000);

            string player = message.Text.Split(' ')[0].Trim();

            if (!File.Exists(Path.Combine(console.Server.WorkingDirectory, "usercache.json"))) return;
            var items = JsonConvert.DeserializeObject<UserCacheItem[]>(File.ReadAllText(Path.Combine(console.Server.WorkingDirectory, "usercache.json")));

            string uuid = null;
            foreach (UserCacheItem item in items)
            {
                if (item.Name == player)
                {
                    uuid = item.UUID;
                }
            }
            if (uuid == null) return;

            bool worldPropertyExists = console.Server.TryGetProperty("level-name", out string world);
            if (!worldPropertyExists) return;

            string playerData = Path.Combine(console.Server.WorkingDirectory, world, $"playerdata/{uuid}.dat");
            if (!File.Exists(playerData)) return;

            var data = new NbtFile(playerData).RootTag.Get<NbtList>("Pos");

            console.SendCommand(CommandHelper.Tellraw("@a", new MinecraftTextElement($"[Death Location] {player} died at approximately ({string.Join(", ", data.Select(x => (int)x.DoubleValue))})", MinecraftColor.light_purple)), true, false);
        }

        public void OnPlayerConnect(IServerConsole console, ServerConnectionMessage message) { }
        public void OnPlayerDisconnect(IServerConsole console, ServerConnectionMessage message) { }

        private string MatchesDeathMessage(string text) => deathMessages.FirstOrDefault(text.Contains);

        private string[] deathMessages =
        {
            "was shot by",
            "was pricked to death",
            "walked into a cactus",
            "roasted in dragon breath",
            "drowned",
            "suffocated in a wall",
            "was squished too much",
            "was squashed by",
            "experienced kinetic energy",
            "blew up",
            "was blown up by",
            "was killed by",
            "hit the ground too hard",
            "fell from a high place",
            "fell off a ladder",
            "fell off some vines",
            "fell out of the water",
            "fell into a patch of",
            "was doomed to fall",
            "fell too far and was finished by",
            "was shot off some vines",
            "was shot off a ladder",
            "was blown from a high place",
            "was squashed by a falling anvil",
            "was squashed by a falling block",
            "was killed by magic",
            "went up in flames",
            "burned to death",
            "was burnt to a crisp whilst fighting",
            "walked into fire whilst fighting",
            "went off with a bang",
            "tried to swim in lava",
            "was struck by lightning",
            "discovered floor was lava",
            "walked into danger zone due to",
            "was slain by",
            "got finished off by",
            "was fireballed by",
            "was killed by even more magic",
            "starved to death",
            "was poked to death by a sweet berry bush‌",
            "was killed while trying to hurt",
            "was impaled by",
            "was speared by",
            "fell out of the world",
            "didn't want to live in the same world as",
            "withered away",
            "was pummeled by"
        };

        private class UserCacheItem
        {
            public string Name { get; set; }
            public string UUID { get; set; }
        }
    }
}
