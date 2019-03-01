using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
using MCServerWrapper.ServerWrapper;
using MCServerWrapper.Utilities;
using System;
using System.Collections.Generic;

namespace ServerWrapperPlugins
{
    /// <inheritdoc />
    public class RecentChat : IPlugin
    {
        public string Name => "Recent Chat Messages";
        public string Description => "Displays chat messages sent in the last 30 minutes to players when they log in";
        private readonly DropOutLinkedList<ServerMessage> chatMessages = new DropOutLinkedList<ServerMessage>(20);

        public void OnStart(MinecraftServer server)
        {
            chatMessages.Clear();
        }

        public void OnExit()
        {
            chatMessages.Clear();
        }

        public void OnChatMessage(MinecraftServer server, ServerChatMessage message)
        {
            chatMessages.AddLast(message);
        }
        public void OnErrorMessage(MinecraftServer server, ServerErrorMessage message) { }
        public void OnSuccessMessage(MinecraftServer server, ServerSuccessMessage message) { }
        public void OnOtherMessage(MinecraftServer server, ServerMessage message) { }

        public void OnPlayerConnect(MinecraftServer server, ServerConnectionMessage message)
        {
            var recentChat = new List<string>();
            foreach (var m in chatMessages)
            {
                if (m.Timestamp > DateTime.Now - TimeSpan.FromMinutes(30))
                {
                    recentChat.Add($"{m.Timestamp:[HH:mm:ss]} {m}");
                }
            }

            if (recentChat.Count > 0)
            {
                server.SendCommand(
                    CommandHelper.Tellraw(
                        message.Username,
                        new MinecraftTextElement
                        {
                            Text = $"Welcome {message.Username}! Here's recent chat messages:",
                            Color = MinecraftColor.gold
                        })
                );

                foreach (string s in recentChat)
                {
                    server.SendCommand(
                        CommandHelper.Tellraw(
                            message.Username,
                            new MinecraftTextElement
                            {
                                Text = s,
                                Color = MinecraftColor.gold
                            })
                    );
                }
            }

            chatMessages.AddLast(message);
        }

        public void OnPlayerDisconnect(MinecraftServer server, ServerConnectionMessage message)
        {
            chatMessages.AddLast(message);
        }
    }
}
