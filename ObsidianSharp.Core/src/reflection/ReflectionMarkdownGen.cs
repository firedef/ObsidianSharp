using Microsoft.CodeAnalysis.CSharp;
using ObsidianSharp.Core.json;
using ObsidianSharp.Core.markdown;

namespace ObsidianSharp.Core.reflection; 

public class ReflectionMarkdownGen {
    public CSharpParseOptions parseOptions = CSharpParseOptions.Default;
    public readonly string outputDir;
    public readonly string inputDir;

    private readonly uint[] _colors = {
        0xcf5757,
        0xcf8557,
        0xcfc957,
        0x89cf57,
        0x57cfa5,
        0x57b1cf,
        0x5771cf,
        0x8557cf,
        0xcf57cf,
        0xcf5785,
        0xcf5769
    };

    public ReflectionMarkdownGen(string outputDir, string inputDir) {
        this.outputDir = outputDir;
        this.inputDir = inputDir;
    }

    public void Generate() {
        if (!CheckPaths()) return;
        
        Namespaces namespaces = new();
        namespaces.ProcessAll(inputDir);
        namespaces.WriteToFiles(outputDir);

        GraphJsonData graphData = new();
        graphData.colorGroups.Add(new("tag:#type", new(1, 2829366)));

        HashSet<string> topLevelNamespaces = new();
        foreach (KeyValuePair<string,Namespace> pair in namespaces.namespaces)
            topLevelNamespaces.Add(pair.Key.Split('.')[0]);

        int i = 0;
        foreach (string topLevelNamespace in topLevelNamespaces) {
            graphData.colorGroups.Add(new($"path:{topLevelNamespace}", new(1, _colors[i % _colors.Length])));
            i++;
        }
        
        graphData.WriteTo(Path.Join(outputDir, ".obsidian", "graph.json"));
    }

    private bool CheckPaths() {
        if (!Directory.Exists(inputDir)) {
            Console.WriteLine($"Input directory is missing: {inputDir}");
            return false;
        }

        if (!Directory.Exists(outputDir)) return true;

        if (CheckOutputDirectory(outputDir)) return true;

        Console.WriteLine("Output directory contains other files");
        return false;
    }

    private bool CheckOutputDirectory(string path) => Directory.GetDirectories(path).All(CheckOutputDirectory) && Directory.GetFiles(path).Select(Path.GetExtension).All(ext => ext is ".md" or ".json");
}