using MCServerWrapper.Messages;

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
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        void OnStart(IServerConsole server);
        /// <summary>
        /// Called when the server exits
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        void OnExit(IServerConsole server);
        /// <summary>
        /// Called when a chat message is sent
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The chat message</param>
        void OnChatMessage(IServerConsole server, ServerChatMessage message);
        /// <summary>
        /// Called when the server receives an error message
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The error message</param>
        void OnErrorMessage(IServerConsole server, ServerErrorMessage message);
        /// <summary>
        /// Called when the server receives a success message
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The success message</param>
        void OnSuccessMessage(IServerConsole server, ServerSuccessMessage message);
        /// <summary>
        /// Called when the server receives a message that isn't otherwise categorized
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The uncategorized message</param>
        void OnOtherMessage(IServerConsole server, ServerMessage message);
        /// <summary>
        /// Called when a player joins the server
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The connection message</param>
        void OnPlayerConnect(IServerConsole server, ServerConnectionMessage message);
        /// <summary>
        /// Called when a player disconnects from the server
        /// </summary>
        /// <param name="server">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The disconnection message</param>
        void OnPlayerDisconnect(IServerConsole server, ServerConnectionMessage message);
    }
}
