using MCServerWrapper.Utilities;
using Newtonsoft.Json;

namespace MCServerWrapper
{
    internal class Config : ConfigFile
    {
        [JsonProperty("jvmArguments")]
        public string JvmArguments { get; set; } = "-Xmx2G -Xms2G";
    }
}
