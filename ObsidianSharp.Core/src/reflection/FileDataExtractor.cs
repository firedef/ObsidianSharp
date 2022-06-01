using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ObsidianSharp.Core.reflection; 

public class FileDataExtractor {
    public string path;
    public CSharpParseOptions parseOptions;
    public List<string> usings = new();
    public string? @namespace;

    public FileDataExtractor(string path, CSharpParseOptions parseOptions) {
        this.path = path;
        this.parseOptions = parseOptions;
    }

    public void Extract() {
        using FileStream fileStream = new(path, FileMode.Open);
        SourceText srcTxt = SourceText.From(fileStream);
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(srcTxt, parseOptions);

        IEnumerable<MemberDeclarationSyntax> memberDeclarations = syntaxTree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();
        foreach (MemberDeclarationSyntax declaration in memberDeclarations) {
            if (declaration is NamespaceDeclarationSyntax ns) {
                @namespace = ns.Name.ToString();
                Console.WriteLine(@namespace);
            }
            if (declaration is FileScopedNamespaceDeclarationSyntax fns) {
                @namespace = fns.Name.ToString();
                Console.WriteLine(@namespace);
            }
        }
        
        foreach (UsingDirectiveSyntax usingDirective in syntaxTree.GetCompilationUnitRoot().Usings) {
            usings.Add(usingDirective.Name.ToString());
        }
    }

    private void ParseNode(SyntaxNode node) {
        NamespaceDeclarationSyntax? namespaceDeclarationSyntax = node.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
        if (namespaceDeclarationSyntax != null) @namespace = namespaceDeclarationSyntax.Name.ToString();
    }
}