using System.Text;

namespace ObsidianSharp.Core.markdown; 

public class MdGenerator {
    private readonly StringBuilder md = new();
    private readonly Dictionary<string, int> footnoteRefs = new();
    private readonly List<string> footnotes = new();

    public virtual void Add(string txt) => md.Append(txt);
    public virtual void AddLine(string txt) => md.Append(txt + "<br/>\n");

    public void NextLine() => AddLine("");
    
    public void AddHeading(string txt, int level = 1) => AddLine(new string('#', level) + " " + txt);
    public void AddHeading(string txt, string id, int level = 1) => AddLine($"{new('#', level)} {txt} {{#{id}}}");
    
    public void AddBold(string txt) => Add($"**{txt}**");
    public void AddItalic(string txt) => Add($"*{txt}*");
    public void AddStrikethrough(string txt) => Add($"~~{txt}~~");
    public void AddSubscript(string txt) => Add($"~{txt}~");
    public void AddSuperscript(string txt) => Add($"^{txt}^");
    
    public void AddQuote(string txt) => AddLine($"> {txt}");
    
    public void AddCode(string txt) => AddLine($"`{txt}`");
    public void AddCode(string txt, string lang) => AddLine($"`{lang} {txt}`");
    
    public void AddRule() => AddLine("---");
    
    public void AddLink(string title, string link) => Add($"[{title}]({link})");
    
    public void AddImage(string title, string link) => Add($"![{title}]({link})");

    public void AddFootnote(string title, string description) {
        footnoteRefs.Add(title, footnotes.Count);
        footnotes.Add($"[^{footnoteRefs[title] + 1}]: " + description);
    }

    public void AddFootnoteRef(string title) => Add($" [^{footnoteRefs[title] + 1}]");


    public virtual string Generate() {
        StringBuilder sb = new(md.ToString());
        sb.AppendLine();

        foreach (string footnote in footnotes) {
            sb.AppendLine(footnote);
        }

        return sb.ToString();
    }
}