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

        switch (oscMessage)
        {
            case float floatValue:
                return new OscFloatMessageDto
                {
                    Address = message.Address,
                    Value = floatValue
                };
            case int intValue:
                return new OscIntMessageDto
                {
                    Address = message.Address,
                    Value = intValue
                };
            case bool boolValue:
                return new OscBoolMessageDto
                {
                    Address = message.Address,
                    Value = boolValue
                };
            default:
                Console.WriteLine(message.ToString());
                return new OscStringMessageDto
                {
                    Address = message.Address,
                    Value = message.ToString()
                };
        }
    }
}