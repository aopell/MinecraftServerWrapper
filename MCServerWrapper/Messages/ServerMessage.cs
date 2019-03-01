using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MCServerWrapper.Messages
{
    public class ServerMessage
    {
        public string Prefix { get; set; }
        public string Text { get; set; }
        public string RawText { get; set; }

        public DateTime Timestamp { get; set; }

        public ServerMessage(string prefix, string text, string rawText)
        {
            Prefix = prefix;
            Text = text;
            RawText = rawText;
        }

        public static ServerMessage Create(string message, Dictionary<string, string> replacements = null)
        {
            int closeSquareBracket = message.IndexOf(']');
            if (closeSquareBracket < 0)
            {
                return new ServerMessage("", message, message);
            }
            int colon = message.Substring(closeSquareBracket).IndexOf(':') + closeSquareBracket;
            return colon < 0
                   ? new ServerMessage("", message, message)
                   : new ServerMessage(message.Substring(0, colon + 1).Trim(), message.Substring(colon + 2).Trim().ReplaceAll(replacements), message);
        }

        public override string ToString() => Text;
    }

    public class ServerChatMessage : ServerMessage
    {
        public string Username { get; set; }
        public string ChatMessage { get; set; }
        public ChatMessageType MessageType { get; set; }

        public ServerChatMessage(string prefix, string text, string rawText) : base(prefix, text, rawText)
        {
            GroupCollection groups = Regex.Match(Text, @"([[<*]) ?(\w{1,16})[>:\] ] ?(.+?)(]$|$)").Groups;
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

        public ServerChatMessage(ServerMessage m) : this(m.Prefix, m.Text, m.RawText) { }

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

        public ServerConnectionMessage(string prefix, string text, string rawText) : base(prefix, text, rawText)
        {
            ConnectionType = text.Contains("lost connection") ? ServerConnectionType.Disconnect : ServerConnectionType.Connect;
            Username = Regex.Match(text, @"\w{1,16}").Value;
        }

        public ServerConnectionMessage(ServerMessage m) : this(m.Prefix, m.Text, m.RawText) { }

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

    public class ServerErrorMessage : ServerMessage
    {
        public ServerErrorMessage(string prefix, string text, string rawText) : base(prefix, text, rawText) { }
        public ServerErrorMessage(ServerMessage m) : this(m.Prefix, m.Text, m.RawText) { }
    }

    public class ServerSuccessMessage : ServerMessage
    {
        public ServerSuccessMessage(string prefix, string text, string rawText) : base(prefix, text, rawText) { }
        public ServerSuccessMessage(ServerMessage m) : this(m.Prefix, m.Text, m.RawText) { }
    }
}
