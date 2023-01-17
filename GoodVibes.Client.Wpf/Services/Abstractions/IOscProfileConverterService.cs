using GoodVibes.Client.Vrchat.Dtos;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IOscProfileConverterService
{
    VrcOscProfileDto DeserializeOscProfile(string profileJson);
}