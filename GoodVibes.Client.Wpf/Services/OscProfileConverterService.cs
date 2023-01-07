using GoodVibes.Client.Vrchat.Dtos;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.Wpf.Services;

public class OscProfileConverterService : IOscProfileConverterService
{
    public VrcOscProfileDto DeserializeOscProfile(string profileJson)
    {
        var profile = JsonConvert.DeserializeObject<VrcOscProfileDto>(profileJson);
        return profile;
    }
}