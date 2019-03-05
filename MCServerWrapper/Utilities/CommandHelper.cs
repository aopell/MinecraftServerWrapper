using Newtonsoft.Json;

namespace MCServerWrapper.Utilities
{
    public static class CommandHelper
    {
        /// <summary>
        /// Forms a tellraw command from the given arguments.
        /// See https://github.com/skylinerw/guides/blob/master/java/text%20component.md
        /// </summary>
        /// <param name="selector">Who should receive this message</param>
        /// <param name="segments">An array of Minecraft JSON text components to display</param>
        /// <returns></returns>
        public static string Tellraw(string selector, params MinecraftTextComponent[] segments)
        {
            return $"tellraw {selector} {JsonConvert.SerializeObject(segments)}";
        }
    }
}
