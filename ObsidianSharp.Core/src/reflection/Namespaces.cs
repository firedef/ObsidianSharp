using Microsoft.CodeAnalysis.CSharp;

namespace ObsidianSharp.Core.reflection; 

public class Namespaces {
    public readonly Dictionary<string, Namespace> namespaces = new();

    public void ProcessAll(string inputPath) {
        Console.WriteLine("Searching for files");
        string[] files = EnumerateFiles(inputPath);
        Console.WriteLine($"Found {files.Length} files");

        for (int i = 0; i < files.Length; i++) 
            ProcessFile(files[i], i, files.Length);

        Console.WriteLine($"Analyzed {files.Length} .cs files");
    }

    private static string[] EnumerateFiles(string path) {
        int matchedCount = 0;
        int analyzedFileCount = 0;
        int analyzedDirectoryCount = 0;

        List<string> matchedFiles = new();
        Stack<string> directories = new();
        
        directories.Push(path);

        while (directories.Count > 0) {
            string dir = directories.Pop();
            
            Console.Write($"#{analyzedDirectoryCount}, found {matchedCount} .cs in {analyzedFileCount} files \r");

            string[] containedDirectories = Directory.GetDirectories(dir);
            foreach (string s in containedDirectories) directories.Push(s);

            string[] files = Directory.GetFiles(dir);
            foreach (string file in files) {
                analyzedFileCount++;
                string ext = Path.GetExtension(file);
                if (ext is not ".cs") continue;
                matchedCount++;
                matchedFiles.Add(file);
            }
            
            analyzedDirectoryCount++;
        }

        Console.WriteLine($"#{analyzedDirectoryCount}, found {matchedCount} in {analyzedFileCount} files");

        return matchedFiles.ToArray();
    }

    public void ProcessFile(string path, int i, int count) {
        Console.Write($"Analyzing {i}/{count} ({((double) i / count * 100):00.00}%) \r");
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
        Console.WriteLine("Writing results to disk");
        int i = 0;
        int c = namespaces.Count;
        
        ClearDirectory(outputPath);
        foreach (KeyValuePair<string,Namespace> pair in namespaces) {
            Console.Write($"Namespace {i}/{c} ({((double) i / c * 100):00.00}%)\r");
            string path = GetFilePath(pair.Value, outputPath);
            WriteToFile(path, pair.Value);

            foreach (TypeData type in pair.Value.types) {
                string filePath = GetTypeFilePath(pair.Value, type.name, outputPath);
                WriteTypeToFile(filePath, type);
            }

            i++;
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