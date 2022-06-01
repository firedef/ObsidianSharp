using Newtonsoft.Json;

namespace ObsidianSharp.Core.json; 

public static class GraphJson {
    public static void WriteTo(this GraphJsonData data, string path) {
        string dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }
}