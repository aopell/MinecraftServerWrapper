using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
using MCServerWrapper.Utilities;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;

namespace SamplePlugins
{
    public class DiscordWebhook : IPlugin
    {
        public string Name => "Discord Webhook";
        public string Description => "Sends a message to a Discord webhook when certain events happen";
        private PluginConfig config { get; }
        private HttpClient client { get; }

        public DiscordWebhook()
        {
            config = ConfigFile.Load<PluginConfig>("Plugins/discord-webhook.json");
            client = new HttpClient();
        }

        public void OnStart(IServerConsole server)
        {
            PostWebhookMessage(new DiscordWebhookMessage("Server is now online"), server);
        }

        public void OnExit(IServerConsole server)
        {
            PostWebhookMessage(new DiscordWebhookMessage("Server is now offline"), server);
        }

        public void OnChatMessage(IServerConsole server, ServerChatMessage message) { }
        public void OnErrorMessage(IServerConsole server, ServerErrorMessage message) { }
        public void OnSuccessMessage(IServerConsole server, ServerSuccessMessage message) { }
        public void OnOtherMessage(IServerConsole server, ServerMessage message) { }

        public void OnPlayerConnect(IServerConsole server, ServerConnectionMessage message)
        {
            PostWebhookMessage(DiscordWebhookMessage.FromServerMessage(message), server);
        }

        public void OnPlayerDisconnect(IServerConsole server, ServerConnectionMessage message)
        {
            PostWebhookMessage(DiscordWebhookMessage.FromServerMessage(message), server);
        }

        private async void PostWebhookMessage(DiscordWebhookMessage message, IServerConsole console)
        {
            if (config.WebhookUrl == null) return;

            try
            {
                await client.PostAsync(config.WebhookUrl, new StringContent(message.ToString(), Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                console.DisplayLine($"[Discord Webhook] Error sending webhook message: {ex}", Color.Red);
            }
        }

        private class PluginConfig : ConfigFile
        {
            [JsonProperty("webhookUrl")]
            public string WebhookUrl { get; set; }
        }

        private class DiscordWebhookMessage
        {
            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonProperty("tts")]
            public bool TTS { get; set; }

            public DiscordWebhookMessage() { }

            public DiscordWebhookMessage(string text)
            {
                Content = text;
            }

            public static DiscordWebhookMessage FromServerMessage(ServerMessage m)
            {
                return new DiscordWebhookMessage
                {
                    Content = m.ToString()
                };
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
