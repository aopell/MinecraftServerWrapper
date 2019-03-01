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

        public void OnStart(IServerConsole console)
        {
            PostWebhookMessage(new DiscordWebhookMessage("Server is now online"), console);
        }

        public void OnExit(IServerConsole console)
        {
            PostWebhookMessage(new DiscordWebhookMessage("Server is now offline"), console);
        }

        public void OnChatMessage(IServerConsole console, ServerChatMessage message) { }
        public void OnErrorMessage(IServerConsole console, ServerErrorMessage message) { }
        public void OnSuccessMessage(IServerConsole console, ServerSuccessMessage message) { }
        public void OnOtherMessage(IServerConsole console, ServerMessage message) { }

        public void OnPlayerConnect(IServerConsole console, ServerConnectionMessage message)
        {
            PostWebhookMessage(DiscordWebhookMessage.FromServerMessage(message), console);
        }

        public void OnPlayerDisconnect(IServerConsole console, ServerConnectionMessage message)
        {
            PostWebhookMessage(DiscordWebhookMessage.FromServerMessage(message), console);
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
