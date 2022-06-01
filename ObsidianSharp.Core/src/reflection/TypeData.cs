using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ObsidianSharp.Core.markdown;

namespace ObsidianSharp.Core.reflection; 

public class TypeData {
    public string @namespace = null!;
    public string type = null!;
    public string name = null!;

    public List<string> methods = new();
    public List<FieldData> fields = new();
    public List<FieldData> properties = new();

    public TypeData(TypeDeclarationSyntax node) {
        ParseTypeNode(node);
    }

    public override string ToString() => $"{type} {name}";
    
    public string ToStringFull() {
        StringBuilder sb = new();
        sb.AppendLine(ToString() + " {");

        foreach (FieldData field in fields)
            sb.AppendLine("  " + field);
        
        foreach (FieldData property in properties)
            sb.AppendLine("  " + property);
        
        foreach (string method in methods)
            sb.AppendLine("  " + method);

        sb.AppendLine("}");
        return sb.ToString();
    }
    
    public string GenerateMarkdown() {
        ObsidianMdGenerator generator = new();
        
        generator.AddHeading(@namespace);
        
        generator.AddHeading("Fields", 2);
        foreach (FieldData field in fields)
            generator.AddLine("  " + field);
        
        generator.AddHeading("Properties", 2);
        foreach (FieldData property in properties)
            generator.AddLine("  " + property);
        
        generator.AddHeading("Methods", 2);
        foreach (string method in methods)
            generator.AddLine("  " + method);
        
        generator.AddInternalLink(@namespace);
        generator.NextLine();
        generator.AddTag(type);
        generator.NextLine();
        generator.AddTag("type");
        generator.NextLine();

        return generator.Generate();
    }

    private void ParseTypeNode(TypeDeclarationSyntax node) {
        type = node.Keyword.ToString();
        name = node.Identifier.ToString();

        foreach (SyntaxNode n in node.DescendantNodes()) 
            ParseDescendantNode(n);
    }

    private void ParseDescendantNode(SyntaxNode node) {
        if (node is MethodDeclarationSyntax meth)
            methods.Add(meth.Identifier.ToString());

        if (node is FieldDeclarationSyntax field) {
            foreach (VariableDeclaratorSyntax decl in field.Declaration.Variables) {
                fields.Add(new(field.Declaration.Type.ToString(), decl.Identifier.ToString()));
            }
        }
        
        if (node is PropertyDeclarationSyntax property) {
            properties.Add(new(property.Type.ToString(), property.Identifier.ToString()));
        }
    }
}