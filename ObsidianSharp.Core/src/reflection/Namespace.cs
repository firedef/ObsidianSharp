using ObsidianSharp.Core.markdown;

namespace ObsidianSharp.Core.reflection; 

public class Namespace {
    public string ns;
    public readonly HashSet<string> usings = new();
    public readonly List<TypeData> types = new();

    public Namespace(string ns) => this.ns = ns;

    public string Generate() {
        ObsidianMdGenerator generator = new();
        
        generator.AddHeading(ns);
        
        foreach (TypeData type in types) {
            generator.AddCode(type.ToString());
        }

        generator.NextLine();

        foreach (string @using in usings) {
            generator.AddInternalLink(@using);
            generator.NextLine();
        }

        return generator.Generate();
    }

    public (string name, string body)[] GenerateTypeFiles() => types.Select(v => (v.name, v.ToStringFull())).ToArray();
}