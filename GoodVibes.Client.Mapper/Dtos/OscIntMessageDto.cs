using GoodVibes.Client.Mapper.Dtos.Abstractions;

namespace GoodVibes.Client.Mapper.Dtos;

public class OscIntMessageDto : IOscTypedMessage
{
    public string Address { get; set; }
    public int Value { get; set; }
    
    public Type Type => typeof(int);
}