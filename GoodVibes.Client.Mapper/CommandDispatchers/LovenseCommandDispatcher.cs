using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using Prism.Events;

namespace GoodVibes.Client.Mapper.CommandDispatchers;

public class LovenseCommandDispatcher
{
    private readonly IEventAggregator _eventAggregator;

    public LovenseCommandDispatcher(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public void DispatchBoolCommand(ToyMappingDto toyMapping, bool value)
    {
        // We got nothing to map bool commands to.
    }

    public void DispatchFloatCommand(ToyMappingDto toyMapping, float value)
    {
        _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Publish(new LovenseCommandEvent
        {
            Command = (LovenseCommandEnum)Enum.Parse(typeof(LovenseCommandEnum), toyMapping.Function!, true),
            Toy = toyMapping.Id,
            Value = value
        });
    }

    public void DispatchIntCommand(ToyMappingDto toyMapping, int value)
    {
        // We got nothing to map int commands to.
    }
}