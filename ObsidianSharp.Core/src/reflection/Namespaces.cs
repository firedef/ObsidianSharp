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
        
        foreach (TypeData type in extractor.types)
            ns.types.Add(type);
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

            foreach (TypeData type in pair.Value.types) {
                string filePath = GetTypeFilePath(pair.Value, type.name, outputPath);
                WriteTypeToFile(filePath, type);
            }
        }
    }
    
    private void ClearDirectory(string dir) {
        if (Directory.Exists(dir)) Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
    }

    private string GetFilePath(Namespace ns, string generalOutputPath) => Path.Join(generalOutputPath, Path.Join(ns.ns.Replace(".", "/"), ns.ns + ".md"));
    private string GetTypeFilePath(Namespace ns, string type, string generalOutputPath) => Path.Join(generalOutputPath, Path.Join(ns.ns.Replace(".", "/"), ns.ns, type + ".md"));

    private void WriteToFile(string filePath, Namespace ns) {
        string txt = ns.Generate();
        WriteTo(filePath, txt);
    }
    
    private void WriteTypeToFile(string filePath, TypeData type) {
        string txt = type.GenerateMarkdown();
        WriteTo(filePath, txt);
    }
    
    private void WriteTo(string path, string text) {
        string dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, text);
    }
}