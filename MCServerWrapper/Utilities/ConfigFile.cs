using Newtonsoft.Json;
using System.IO;

namespace MCServerWrapper.Utilities
{
    public abstract class ConfigFile
    {
        [JsonIgnore]
        public string FilePath { get; set; }

        public virtual void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
