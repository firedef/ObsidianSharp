using Microsoft.CodeAnalysis.CSharp;

namespace ObsidianSharp.Core.reflection; 

public class Namespaces {
    public readonly Dictionary<string, Namespace> namespaces = new();

    public void ProcessAll(string inputPath) {
        string[] files = Directory.EnumerateFiles(inputPath, "*.cs", SearchOption.AllDirectories).ToArray();
        foreach (string file in files) ProcessFile(file);
    }

    public void ProcessFile(string path) {
        FileDataExtractor extractor = new(path, CSharpParseOptions.Default);
        extractor.Extract();

        Namespace ns = GetOrCreateNamespace(extractor.@namespace ?? "global");
        foreach (string @using in extractor.usings)
            ns.usings.Add(@using);
    }

    private Namespace GetOrCreateNamespace(string name) {
        if (!namespaces.ContainsKey(name)) namespaces.Add(name, new(name));
        return namespaces[name];
    }

    public void WriteToFiles(string outputPath) {
        ClearDirectory(outputPath);
        foreach (KeyValuePair<string,Namespace> pair in namespaces) {
            string path = GetFilePath(pair.Value, outputPath);
            WriteToFile(path, pair.Value);
        }
    }
    
    private void ClearDirectory(string dir) {
        if (Directory.Exists(dir)) Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
    }

    private string GetFilePath(Namespace ns, string generalOutputPath) {
        return Path.Join(generalOutputPath, Path.Join(ns.ns.Replace(".", "/"), ns.ns + ".md"));
    }

    private void WriteToFile(string filePath, Namespace ns) {
        string txt = ns.Generate();
        WriteTo(filePath, txt);
    }
    
    private void WriteTo(string path, string text) {
        string dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, text);
    }
}