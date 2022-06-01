using ObsidianSharp.Core.markdown;

namespace ObsidianSharp.Core.reflection; 

public class Namespace {
    public string ns;
    public readonly HashSet<string> usings = new();

    public Namespace(string ns) => this.ns = ns;

    public string Generate() {
        ObsidianMdGenerator generator = new();
        
        generator.AddHeading(ns);

        foreach (string @using in usings) {
            generator.AddInternalLink(@using);
            generator.NextLine();
        }

        return generator.Generate();
    }
}