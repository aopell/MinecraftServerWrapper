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

        public static T Load<T>(string path) where T : ConfigFile, new()
        {
            if (!File.Exists(path)) return new T();

            var c = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            c.FilePath = path;
            return c;

        }
    }
}
