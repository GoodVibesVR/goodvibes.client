using GoodVibes.Client.Mapper.Dtos.Abstractions;

namespace GoodVibes.Client.Mapper.Dtos;

public class OscBoolMessageDto : IOscTypedMessage
{
    public string? Address { get; set; }
    public bool Value { get; set; }
    
    public Type Type => typeof(bool);
}