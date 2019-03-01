using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MCServerWrapper.Utilities
{
    public class MinecraftTextElement
    {
        [JsonProperty("text")] public string Text { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("color")] public MinecraftColor Color { get; set; }

        [JsonProperty("bold")] public bool Bold { get; set; }

        [JsonProperty("italic")] public bool Italic { get; set; }

        [JsonProperty("underlined")] public bool Underlined { get; set; }

        [JsonProperty("obfuscated")] public bool Obfuscated { get; set; }

        [JsonProperty("strikethrough")] public bool Strikethrough { get; set; }

        [JsonProperty("extra")] public MinecraftTextElement[] Extra { get; set; } = new MinecraftTextElement[0];

        public MinecraftTextElement() { }

        public MinecraftTextElement(
            string text = "",
            MinecraftColor color = MinecraftColor.white,
            bool bold = false,
            bool italic = false,
            bool underlined = false,
            bool obfuscated = false,
            bool strikethrough = false,
            MinecraftTextElement[] extra = null)
        {
            Text = text;
            Color = color;
            Bold = bold;
            Italic = italic;
            Underlined = underlined;
            Obfuscated = obfuscated;
            Strikethrough = strikethrough;
            Extra = extra ?? new MinecraftTextElement[0];
        }

        public static MinecraftTextElement[] FromJson(string json)
        {
            return JObject.Parse(json).Type == JTokenType.Array
                       ? JsonConvert.DeserializeObject<MinecraftTextElement[]>(json)
                       : new[] { JsonConvert.DeserializeObject<MinecraftTextElement>(json) };
        }

        public override string ToString() => Extra == null ? Text : string.Join("", Text, string.Join("", (object[])Extra));
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
