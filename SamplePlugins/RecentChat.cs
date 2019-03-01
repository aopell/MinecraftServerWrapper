using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
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

        public void OnStart(IServerConsole console)
        {
            chatMessages.Clear();
        }

        public void OnExit(IServerConsole console)
        {
            chatMessages.Clear();
        }

        public void OnChatMessage(IServerConsole console, ServerChatMessage message)
        {
            chatMessages.AddLast(message);
        }
        public void OnErrorMessage(IServerConsole console, ServerErrorMessage message) { }
        public void OnSuccessMessage(IServerConsole console, ServerSuccessMessage message) { }
        public void OnOtherMessage(IServerConsole console, ServerMessage message) { }

        public void OnPlayerConnect(IServerConsole console, ServerConnectionMessage message)
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
                console.SendCommand(
                    CommandHelper.Tellraw(
                        message.Username,
                        new MinecraftTextElement
                        {
                            Text = $"Welcome {message.Username}! Here are some recent chat messages:",
                            Color = MinecraftColor.gold
                        }
                    ),
                    displayInConsole: false,
                    addToHistory: false
                );

                foreach (string s in recentChat)
                {
                    console.SendCommand(
                        CommandHelper.Tellraw(
                            message.Username,
                            new MinecraftTextElement
                            {
                                Text = s,
                                Color = MinecraftColor.gold
                            }
                        ),
                        displayInConsole: false,
                        addToHistory: false
                    );
                }
            }

            chatMessages.AddLast(message);
        }

        public void OnPlayerDisconnect(IServerConsole console, ServerConnectionMessage message)
        {
            chatMessages.AddLast(message);
        }
    }
}
