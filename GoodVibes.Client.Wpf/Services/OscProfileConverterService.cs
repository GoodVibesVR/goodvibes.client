using GoodVibes.Client.Vrchat.Dtos;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.Wpf.Services;

public class OscProfileConverterService : IOscProfileConverterService
{
    public OscProfileDto DeserializeOscProfile(string profileJson)
    {
        var profile = JsonConvert.DeserializeObject<OscProfileDto>(profileJson);
        return profile;
    }
}