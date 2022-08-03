namespace GoodVibes.Client.Cache.Models;

public class AvatarCache
{
    public string? Name { get; set; }
    /// <summary>
    /// Key: OSC Address
    /// Value: OSC Mappings
    /// </summary>
    public Dictionary<string, List<MappedFunctionsCache>> OscMappings { get; set; }

    public AvatarCache()
    {
        OscMappings = new Dictionary<string, List<MappedFunctionsCache>>();
    }
}