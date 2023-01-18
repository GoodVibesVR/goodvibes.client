using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.PiShock.Constants;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Prism.Events;

namespace GoodVibes.Client.Mapper.CommandDispatchers;

public class PiShockCommandDispatcher
{
    private readonly IEventAggregator _eventAggregator;

    public PiShockCommandDispatcher(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public void DispatchBoolCommand(ToyMappingDto toyMapping, bool value)
    {
        var properCommand = Enum.TryParse(typeof(PiShockCommandEnum), toyMapping.Function!, true, out var command);
        if (!properCommand) return;

        switch (command)
        {
            case PiShockCommandEnum.Shock:
                if (!value) return;
                _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
                {
                    Command = PiShockCommandEnum.Shock,
                    ShareCode = toyMapping.Id
                });
                break;
            case PiShockCommandEnum.MiniShock:
                if (!value) return;
                _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
                {
                    Command = PiShockCommandEnum.MiniShock,
                    ShareCode = toyMapping.Id
                });
                break;
            case PiShockCommandEnum.Vibrate:
                if (!value) return;
                _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
                {
                    Command = PiShockCommandEnum.Vibrate,
                    ShareCode = toyMapping.Id
                });
                break;
            case PiShockCommandEnum.Beep:
                if (!value) return;
                _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
                {
                    Command = PiShockCommandEnum.Beep,
                    ShareCode = toyMapping.Id
                });
                break;
            case PiShockCommandEnum.Pause:
                _eventAggregator.GetEvent<PausePiShockEventCarrier>().Publish(new PausePiShockEvent()
                {
                    ShareCode = toyMapping.Id,
                    Pause = value
                });
                break;
            default:
                return;
        }
    }

    public void DispatchFloatCommand(ToyMappingDto toyMapping, float value)
    {
        switch (toyMapping.Function!)
        {
            case PiShockMappableFunctionsConstant.Intensity:
                _eventAggregator.GetEvent<PiShockIntensityChangedEventCarrier>().Publish(new PiShockIntensityChangedEvent()
                {
                    ToyId = toyMapping.Id,
                    Intensity = value
                });
                break;
            case PiShockMappableFunctionsConstant.Duration:
                _eventAggregator.GetEvent<PiShockDurationChangedEventCarrier>().Publish(new PiShockDurationChangedEvent()
                {
                    ToyId = toyMapping.Id,
                    Duration = value
                });
                break;
        }
    }

    public void DispatchIntCommand(ToyMappingDto toyMapping, int value)
    {
        // We got nothing to map int commands to.
    }
}