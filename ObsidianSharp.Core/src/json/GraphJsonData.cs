using System.Text.Json.Serialization;

namespace ObsidianSharp.Core.json; 

public class GraphJsonData {
    [JsonPropertyName("collapse-filter")]
    public bool collapseFilter = true;
    
    [JsonPropertyName("search")]
    public string search = "";
    
    [JsonPropertyName("showTags")]
    public bool showTags = false;
    
    [JsonPropertyName("showAttachments")]
    public bool showAttachments = false;
    
    [JsonPropertyName("hideUnresolved")]
    public bool hideUnresolved = false;
    
    [JsonPropertyName("showOrphans")]
    public bool showOrphans = true;
    
    [JsonPropertyName("collapse-color-groups")]
    public bool collapseColorGroups = true;
    
    [JsonPropertyName("colorGroups")]
    public List<ColorGroup> colorGroups = new();
    
    [JsonPropertyName("collapse-display")]
    public bool collapseDisplay = true;
    
    [JsonPropertyName("showArrow")]
    public bool showArrow = false;
    
    [JsonPropertyName("textFadeMultiplier")]
    public float textFadeMultiplier = 0;
    
    [JsonPropertyName("nodeSizeMultiplier")]
    public float nodeSizeMultiplier = 1;
    
    [JsonPropertyName("lineSizeMultiplier")]
    public float lineSizeMultiplier = 1;

    [JsonPropertyName("collapse-forces")]
    public bool collapseForces = false;

    [JsonPropertyName("centerStrength")]
    public float centerStrength = 0.0427350427350427f;
    
    [JsonPropertyName("repelStrength")]
    public float repelStrength = 10;
    
    [JsonPropertyName("linkStrength")]
    public float linkStrength = 0.760683760683761f;
    
    [JsonPropertyName("linkDistance")]
    public float linkDistance = 30;
    
    [JsonPropertyName("scale")]
    public float scale = 0.05696679276618759f;
    
    [JsonPropertyName("close")]
    public bool close = false;
}