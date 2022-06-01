namespace ObsidianSharp.Core.markdown; 

public class ObsidianMdGenerator : MdGenerator {
    private bool calloutStarted = false;
    
    public void AddInternalLink(string link) => Add($"[[{link}]]");
    public void AddInternalLink(string link, string heading) => Add($"[[{link}#{heading}]]");
    
    public void AddIFrame(string link) => AddLine($"<iframe src=\"{link}\"></iframe>");
    public void AddIFrame(string link, string style) => AddLine($"<iframe src=\"{link}\" style=\"{style}\"></iframe>");

    public override void Add(string txt) {
        if (calloutStarted) txt = $"> {txt}";
        base.Add(txt);
    }

    public override void AddLine(string txt) {
        if (calloutStarted) txt = $"> {txt}";
        base.AddLine(txt);
    }

    public void BeginCallout(string type) {
        if (calloutStarted) throw new("Callout already started");
        calloutStarted = true;
        Add($"[!{type}]");
    }
    
    public void BeginCallout(CalloutType type) {
        if (calloutStarted) throw new("Callout already started");
        calloutStarted = true;
        Add($"[!{type.ToString().ToUpper()}]\n");
    }

    public void EndCallout() {
        if (!calloutStarted) throw new("Callout is not started");
        calloutStarted = false;
    }

    public override string Generate() {
        calloutStarted = false;
        return base.Generate();
    }
}

public enum CalloutType {
    info,
    tip,
    faq,
}