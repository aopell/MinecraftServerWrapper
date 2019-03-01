using Newtonsoft.Json;

namespace MCServerWrapper.Utilities
{
    public static class CommandHelper
    {
        public static string Tellraw(string selector, params MinecraftTextElement[] segments)
        {
            return $"tellraw {selector} {JsonConvert.SerializeObject(segments)}";
        }
    }
}
