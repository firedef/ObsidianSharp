namespace ObsidianSharp.Core.reflection; 

public class FieldData {
    public string type;
    public string name;

    public FieldData(string type, string name) {
        this.type = type;
        this.name = name;
    }
    
    public override string ToString() => $"{type} {name}";
}