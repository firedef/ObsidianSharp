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
        Namespaces namespaces = new();
        namespaces.ProcessAll(inputDir);
        namespaces.WriteToFiles(outputDir);
    }
}