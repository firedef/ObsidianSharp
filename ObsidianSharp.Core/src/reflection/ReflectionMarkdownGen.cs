using Microsoft.CodeAnalysis.CSharp;
using ObsidianSharp.Core.markdown;

namespace ObsidianSharp.Core.reflection; 

public class ReflectionMarkdownGen {
    public CSharpParseOptions parseOptions = CSharpParseOptions.Default;
    public readonly string outputDir;
    public readonly string inputDir;

    public ReflectionMarkdownGen(string outputDir, string inputDir) {
        this.outputDir = outputDir;
        this.inputDir = inputDir;
    }

    public void Generate() {
        Console.WriteLine("--- Clearing obsidian directory...");
        ClearDirectory(outputDir);
        
        Console.WriteLine("--- Searching .cs files...");
        string[] files = GetFiles(inputDir);
        Console.WriteLine($"Found {files.Length} .cs files...");
        
        Console.WriteLine("--- Generating files...");
        GenerateAll(files);
        
        Console.WriteLine("--- Done!");
    }

    private void ClearDirectory(string dir) {
        if (Directory.Exists(dir)) Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
    }

    private string[] GetFiles(string path) => Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories).ToArray();

    private void GenerateAll(IEnumerable<string> paths) {
        foreach (string path in paths) {
            string relativePath = path.Replace(inputDir, "");
            GenerateAndWriteFile(relativePath);
        }
    }

    private void GenerateAndWriteFile(string relativePath) {
        string txt = GenerateFromFile(Path.Join(inputDir, relativePath));
        WriteTo(Path.Join(outputDir, relativePath.Replace(".cs", ".md")), txt);
    }
    
    private void WriteTo(string path, string text) {
        string dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, text);
    }
    
    private string GenerateFromFile(string path) {
        string fileName = Path.GetFileName(path);
        ObsidianMdGenerator generator = new();

        FileDataExtractor extractor = new(path, parseOptions);
        extractor.Extract();
        
        generator.AddHeading(fileName);

        foreach (string @using in extractor.usings) {
            generator.AddInternalLink(@using);
            generator.NextLine();
        }

        return generator.Generate();
    }
}