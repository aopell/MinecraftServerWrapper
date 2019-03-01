using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace MCServerWrapper.Messages
{
    public static class CommandHelper
    {
        public static string Tellraw(string selector, params MinecraftTextElement[] segments)
        {
            return $"tellraw {selector} {JsonConvert.SerializeObject(segments)}";
        }
    }

    public class MinecraftTextElement
    {
        [J("text")] public string Text { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [J("color")] public MinecraftColor Color { get; set; }

        [J("bold")] public bool Bold { get; set; }

        [J("italic")] public bool Italic { get; set; }

        [J("underlined")] public bool Underlined { get; set; }

        [J("obfuscated")] public bool Obfuscated { get; set; }

        [J("strikethrough")] public bool Strikethrough { get; set; }

        public MinecraftTextElement(
            string text = "",
            MinecraftColor color = MinecraftColor.white,
            bool bold = false,
            bool italic = false,
            bool underlined = false,
            bool obfuscated = false,
            bool strikethrough = false)
        {
            Text = text;
            Color = color;
            Bold = bold;
            Italic = italic;
            Underlined = underlined;
            Obfuscated = obfuscated;
            Strikethrough = strikethrough;
        }
    }

    public enum MinecraftColor
    {
        black,
        dark_blue,
        dark_green,
        dark_aqua,
        dark_red,
        dark_purple,
        gold,
        gray,
        dark_gray,
        blue,
        green,
        aqua,
        red,
        light_purple,
        yellow,
        white
    }
}
