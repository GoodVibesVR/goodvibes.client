using GoodVibes.Client.Mapper.Dtos.Abstractions;

namespace GoodVibes.Client.Mapper.Dtos;

public class OscStringMessageDto : IOscTypedMessage
{
    public string Address { get; set; }
    public string Value { get; set; }
    
    public Type Type => typeof(string);
}