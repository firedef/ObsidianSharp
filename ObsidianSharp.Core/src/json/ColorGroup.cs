namespace ObsidianSharp.Core.json; 

public class ColorGroup {
    public string query;
    public Color color;

    public ColorGroup(string query, Color color) {
        this.query = query;
        this.color = color;
    }
}

public struct Color {
    public float a;
    public uint rgb;

    public Color(float a, uint rgb) {
        this.a = a;
        this.rgb = rgb;
    }
}