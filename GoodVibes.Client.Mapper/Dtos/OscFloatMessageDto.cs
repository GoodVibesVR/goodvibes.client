using GoodVibes.Client.Mapper.Dtos.Abstractions;

namespace GoodVibes.Client.Mapper.Dtos;

public class OscFloatMessageDto : IOscTypedMessage
{
    public string Address { get; set; }
    public float Value { get; set; }

    public Type Type => typeof(float);
}