using System;
using System.Text.RegularExpressions;

namespace MCServerWrapper.Messages
{
    public class ServerMessage
    {
        public string Prefix { get; set; }
        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public ServerMessage(string prefix, string text)
        {
            Prefix = prefix;
            Text = text;
            Timestamp = DateTime.Now;
        }

        public static ServerMessage Create(string message)
        {
            int closeSquareBracket = message.IndexOf(']');
            if (closeSquareBracket < 0)
            {
                return new ServerMessage("", message);
            }
            int colon = message.Substring(closeSquareBracket).IndexOf(':') + closeSquareBracket;
            return colon < 0
                   ? new ServerMessage("", message)
                   : new ServerMessage(message.Substring(0, colon + 1).Trim(), message.Substring(colon + 2).Trim());
        }

        public override string ToString() => Text;
    }

    public class ServerErrorMessage : ServerMessage
    {
        public ServerErrorMessage(string prefix, string text) : base(prefix, text) { }
        public ServerErrorMessage(ServerMessage m) : this(m.Prefix, m.Text) { }
    }

    public class ServerSuccessMessage : ServerMessage
    {
        public ServerSuccessMessage(string prefix, string text) : base(prefix, text) { }
        public ServerSuccessMessage(ServerMessage m) : this(m.Prefix, m.Text) { }
    }

    public class ServerChatMessage : ServerMessage
    {
        public string Username { get; set; }
        public string ChatMessage { get; set; }
        public ChatMessageType MessageType { get; set; }

        public ServerChatMessage(string prefix, string text) : base(prefix, text)
        {
            /* TODO: Issues with custom scoreboard prefixes/suffixes, the regex will no longer match properly
             * TODO: This also creates problems with capturing the username since prefixes/suffixes can be arbitrary strings
             * TODO: There may be no resolution to this. Only thought could be comparing "username" to a list of logged in users
             * TODO: However this has its own problems since it may display pre-/suffixes in join messages (though maybe not the UUID verification message)
             * TODO: Will probably need to split into separate regexes for each message type */

            GroupCollection groups = Regex.Match(text, @"([[<*]) ?(\w{1,16})[>:\] ] ?(.+?)(]$|$)").Groups;
            Username = groups[2].Value;
            ChatMessage = groups[3].Value;

            switch (groups[1].Value)
            {
                case "<":
                    MessageType = ChatMessageType.Player;
                    break;
                case "[":
                    MessageType = string.IsNullOrEmpty(groups[4].Value) ? ChatMessageType.Say : ChatMessageType.Command;
                    break;
                case "*":
                    MessageType = ChatMessageType.Me;
                    break;
                default:
                    MessageType = ChatMessageType.Unknown;
                    break;
            }
        }

        public ServerChatMessage(ServerMessage m) : this(m.Prefix, m.Text) { }

        public override string ToString()
        {
            switch (MessageType)
            {
                case ChatMessageType.Player:
                    return $"<{Username}> {ChatMessage}";
                case ChatMessageType.Command:
                    return $"[{Username}: {ChatMessage}]";
                case ChatMessageType.Say:
                    return $"[{Username}] {ChatMessage}";
                case ChatMessageType.Me:
                    return $"* {Username} {ChatMessage}";
                case ChatMessageType.Unknown:
                    return Text;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum ChatMessageType
        {
            Player,
            Command,
            Say,
            Me,
            Unknown
        }

    }

    public class ServerConnectionMessage : ServerMessage
    {
        public string Username { get; set; }
        public ServerConnectionType ConnectionType { get; set; }

        public ServerConnectionMessage(string prefix, string text) : base(prefix, text)
        {
            ConnectionType = text.Contains("lost connection") ? ServerConnectionType.Disconnect : ServerConnectionType.Connect;
            Username = Regex.Match(text, @"\w{1,16}").Value;
        }

        public ServerConnectionMessage(ServerMessage m) : this(m.Prefix, m.Text) { }

        public override string ToString()
        {
            return $"{Username} {(ConnectionType == ServerConnectionType.Connect ? "joined the game" : "left the game")}";
        }

        public string ToFullString() => base.ToString();

        public enum ServerConnectionType
        {
            Connect,
            Disconnect
        }
    }
}
