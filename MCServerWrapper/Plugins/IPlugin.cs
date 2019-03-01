using MCServerWrapper.Messages;
using MCServerWrapper.ServerWrapper;

namespace MCServerWrapper.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Short description of what the plugin does
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Called when the server starts
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        void OnStart(MinecraftServer server);
        /// <summary>
        /// Called when the server exits
        /// </summary>
        void OnExit();
        /// <summary>
        /// Called when a chat message is sent
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The chat message</param>
        void OnChatMessage(MinecraftServer server, ServerChatMessage message);
        /// <summary>
        /// Called when the server receives an error message
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The error message</param>
        void OnErrorMessage(MinecraftServer server, ServerErrorMessage message);
        /// <summary>
        /// Called when the server receives a success message
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The success message</param>
        void OnSuccessMessage(MinecraftServer server, ServerSuccessMessage message);
        /// <summary>
        /// Called when the server receives a message that isn't otherwise categorized
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The uncategorized message</param>
        void OnOtherMessage(MinecraftServer server, ServerMessage message);
        /// <summary>
        /// Called when a player joins the server
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The connection message</param>
        void OnPlayerConnect(MinecraftServer server, ServerConnectionMessage message);
        /// <summary>
        /// Called when a player disconnects from the server
        /// </summary>
        /// <param name="server">A server wrapper providing an interface to the server process</param>
        /// <param name="message">The disconnection message</param>
        void OnPlayerDisconnect(MinecraftServer server, ServerConnectionMessage message);
    }
}
