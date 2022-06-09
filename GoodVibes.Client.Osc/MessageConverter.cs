using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.Dtos.Abstractions;
using Rug.Osc.Core;

namespace GoodVibes.Client.Osc;

public static class MessageConverter
{
    public static IOscTypedMessage? ToDto(this OscMessage message)
    {
        if (message.Count > 1 || message.Count == 0)
            return null;

        var oscMessage = message.First();

        var floatValue = oscMessage as float?;
        if (floatValue != null)
            return new OscFloatMessageDto
            {
                Address = message.Address,
                Value = floatValue.Value
            };

        var intValue = oscMessage as int?;
        if (intValue != null)
        {
            return new OscIntMessageDto
            {
                Address = message.Address,
                Value = intValue.Value
            };
        }

        var boolValue = oscMessage as bool?;
        if (boolValue != null)
            return new OscBoolMessageDto
            {
                Address = message.Address,
                Value = boolValue.Value
            };

        return new OscStringMessageDto
        {
            Address = message.Address,
            Value = message.ToString()
        };
    }
}