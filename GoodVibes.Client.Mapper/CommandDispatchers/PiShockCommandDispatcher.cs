using GoodVibes.Client.Common.Enums;
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
        if (toyMapping.ToyType == ToyTypeEnum.PiShock)
        {
            var piShockSuccess = Enum.TryParse(typeof(PiShockCommandEnum), toyMapping.Function!, true, out var piShockCommand);
            if (!piShockSuccess) return;

            switch (piShockCommand)
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

            return;
        }

        var piVaultSuccess = Enum.TryParse(typeof(PiVaultCommandEnum), toyMapping.Function!, true, out var piVaultCommand);
        if (!piVaultSuccess) return;

        switch (piVaultCommand)
        {
            case PiVaultCommandEnum.Unlock:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.Unlock
                });
                break;
            case PiVaultCommandEnum.ClearSession:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.ClearSession
                });
                break;
            case PiVaultCommandEnum.AddMinutes:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.AddMinutes
                });
                break;
            case PiVaultCommandEnum.AddHours:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.AddHours
                });
                break;
            case PiVaultCommandEnum.AddDays:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.AddDays
                });
                break;
            case PiVaultCommandEnum.RemoveMinutes:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.RemoveMinutes
                });
                break;
            case PiVaultCommandEnum.RemoveHours:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.RemoveHours
                });
                break;
            case PiVaultCommandEnum.RemoveDays:
                if (!value) return;
                _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Command = PiVaultCommandEnum.RemoveDays
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
            case PiVaultMappableFunctionsConstant.AmountToAddRemove:
                var amount = value * 100;
                _eventAggregator.GetEvent<PiVaultAmountToAddOrRemoveChangedEventCarrier>().Publish(new PiVaultAmountToAddOrRemoveChangedEvent()
                {
                    ApiKey = new Guid(toyMapping.Id!),
                    Amount = (int)Math.Round((double)value * 100)
                });
                break;
        }
    }

    public void DispatchIntCommand(ToyMappingDto toyMapping, int value)
    {
        // We got nothing to map int commands to.
    }
}