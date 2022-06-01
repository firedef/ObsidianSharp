using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ObsidianSharp.Core.reflection; 

public class FileDataExtractor {
    public string path;
    public CSharpParseOptions parseOptions;
    public List<string> usings = new();

    public FileDataExtractor(string path, CSharpParseOptions parseOptions) {
        this.path = path;
        this.parseOptions = parseOptions;
    }

    public void Extract() {
        using FileStream fileStream = new(path, FileMode.Open);
        SourceText srcTxt = SourceText.From(fileStream);
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(srcTxt, parseOptions);
        
        foreach (UsingDirectiveSyntax usingDirective in syntaxTree.GetCompilationUnitRoot().Usings) {
            usings.Add(usingDirective.Name.ToString());
        }
    }
}