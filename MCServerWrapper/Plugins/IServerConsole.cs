using MCServerWrapper.ServerWrapper;
using System;
using System.Drawing;

namespace MCServerWrapper.Plugins
{
    public interface IServerConsole
    {
        /// <summary>
        /// Provides access to the underlying server process
        /// </summary>
        MinecraftServer Server { get; }

        /// <summary>
        /// Starts the server
        /// </summary>
        void StartServer();

        /// <summary>
        /// Stops the server
        /// </summary>
        void StopServer();

        /// <summary>
        /// Restarts the server
        /// </summary>
        void RestartServer();

        /// <summary>
        /// Adds a text line to the console display with the given foreground color
        /// </summary>
        /// <param name="text">The text to add to the console display</param>
        /// <param name="color">The foreground color of the text</param>
        /// <param name="time">The time to display for the message, uses <see cref="DateTime.Now"/> by default</param>
        /// <param name="displayTime">Whether or not to display the time when logging the message</param>
        void DisplayLine(string text, Color color, DateTime? time = null, bool displayTime = true);

        /// <summary>
        /// Sends a command to the underlying server and optionally displays it in the console window and adds it to command history
        /// </summary>
        /// <param name="command">The command to send to the server</param>
        /// <param name="displayInConsole">Whether or not to display the sent command in the console</param>
        /// <param name="addToHistory">Whether or not to add this command to sent command history</param>
        void SendCommand(string command, bool displayInConsole = true, bool addToHistory = true);
    }
}
