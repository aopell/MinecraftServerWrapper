using System.Collections.Generic;
using System.Linq;

namespace MCServerWrapper.Messages
{
    internal static class ServerOutputParser
    {
        /// <summary>
        /// Returns the appropriate subclass of <see cref="ServerMessage" /> depending on the contents of <paramref name="inputMessage"/>
        /// </summary>
        /// <param name="inputMessage">Standard output message received from the Minecraft server process</param>
        public static ServerMessage DetermineMessageType(string inputMessage, Dictionary<string, string> replacements)
        {
            ServerMessage message = ServerMessage.Create(inputMessage);

            if (string.IsNullOrEmpty(message.Text))
            {
                return message;
            }

            if (message.Text[0] == '<' || message.Text[0] == '[' || message.Text[0] == '*') // "<user> Chat message" or "[user: Set gamemode to Creative]" or "[user] Say command message" or "* user sings"
            {
                return new ServerChatMessage(message, replacements);
            }
            else if (message.Text.ContainsAny("logged in", "lost connection"))
            {
                return new ServerConnectionMessage(message);
            }
            else if (message.Prefix.ContainsAny("WARN", "ERROR") || message.Text.ContainsAny("moved too quickly!", "with pending removal and duplicate UUID", "server overloaded?", "is sending move packets too frequently", "exception was", "FAILED TO BIND", "Perhaps a server", "stopping server", "Stopping server", "Stopping the server", "Ambiguity between arguments"))
            {
                return new ServerErrorMessage(message);
            }
            else if (message.Text.ContainsAny("Done (", "Starting minecraft server", "Starting Minecraft server", "has made the advancement"))
            {
                return new ServerSuccessMessage(message);
            }

            return message;
        }
    }

    internal static class ParserExtensions
    {
        /// <summary>
        /// Determines whether any given substring appears in the target <see cref="string" />
        /// </summary>
        /// <param name="s"></param>
        /// <param name="substrings">Substrings to check</param>
        public static bool ContainsAny(this string s, params string[] substrings) => substrings.Any(s.Contains);

        /// <summary>
        /// Replaces all keys in the provided dictionary with their associated values
        /// </summary>
        /// <param name="s"></param>
        /// <param name="replacements">A dictionary with values being the replacement for the associated keys</param>
        /// <returns></returns>
        public static string ReplaceAll(this string s, Dictionary<string, string> replacements)
        {
            if (replacements == null) return s;

            foreach (string r in replacements.Keys)
            {
                s = s.Replace(r, replacements[r]);
            }

            return s;
        }
    }
}
