using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Common.Extensions;
using GoodVibes.Client.Mapper.CommandDispatchers;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.Dtos.Abstractions;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using Prism.Events;

namespace GoodVibes.Client.Mapper;

public class AvatarMapperClient
{
    private readonly IEventAggregator _eventAggregator;
    private readonly LovenseCommandDispatcher _lovenseCommandDispatcher;
    private readonly PiShockCommandDispatcher _piShockCommandDispatcher;

    private readonly Dictionary<string, List<ToyMappingDto>> _mappings;

    public AvatarMapperClient(IEventAggregator eventAggregator, 
        LovenseCommandDispatcher lovenseCommandDispatcher, PiShockCommandDispatcher piShockCommandDispatcher)
    {
        _eventAggregator = eventAggregator;
        _lovenseCommandDispatcher = lovenseCommandDispatcher;
        _piShockCommandDispatcher = piShockCommandDispatcher;

        _mappings = new Dictionary<string, List<ToyMappingDto>>();
    }

    private string buildOscAddress(string address)
    {
        if (address.ToLower().Contains("/avatar/parameters/"))
        {
            return address;
        }

        return $"/avatar/parameters/{address}";
    }

    public void AddMapping(string oscAddress, ToyMappingDto toyMappingDto)
    {
        oscAddress = buildOscAddress(oscAddress);
        if (_mappings.TryGetValue(oscAddress, out var mappingDtos))
        {
            mappingDtos.Add(toyMappingDto);
        }
        else
        {
            _mappings.Add(oscAddress, new List<ToyMappingDto> { toyMappingDto });
        }
    }

    public void RemoveMapping(string oscAddress, ToyMappingDto toyMappingDto)
    {
        oscAddress = buildOscAddress(oscAddress);
        if (_mappings.TryGetValue(oscAddress, out var mappingDtos))
        {
            var mapping =
                mappingDtos.RemoveAll(m => m.Id == toyMappingDto.Id && m.Function == toyMappingDto.Function);
            if (mappingDtos.Count == 0)
            {
                _mappings.Remove(oscAddress);
            }
        }
    }

    public void RemoveMapping(string oscAddress)
    {
        oscAddress = buildOscAddress(oscAddress);
        if (_mappings.TryGetValue(oscAddress, out var mappingDtos))
        {
            _mappings.Remove(oscAddress);
        }
    }

    public void ChangeOrAddMappingAddress(string oldOscAddress, string newOscAddress)
    {
        oldOscAddress = buildOscAddress(oldOscAddress);
        newOscAddress = buildOscAddress(newOscAddress);

        var exists = _mappings.TryGetValue(oldOscAddress, out var mapping);
        if (exists)
        {
            _mappings.Remove(oldOscAddress);
            _mappings.Add(newOscAddress, mapping!);

            return;
        }

        _mappings.Add(newOscAddress, new List<ToyMappingDto>());
    }

    public void ChangeMappings(IEnumerable<MappingDto> oldMappings, IEnumerable<MappingDto> newMappings)
    {
        _mappings.Clear();
        
        foreach (var newMapping in newMappings)
        {
            var oscAddress = buildOscAddress(newMapping.OscAddress!);
            _mappings.Add(oscAddress, newMapping.ToyMappings!);
        }
    }

    public void MapAndPublishEvent(IOscTypedMessage message)
    {
        switch (message)
        {
            case OscBoolMessageDto dto:
                MapAndDispatchBoolEvent(dto);
                break;
            case OscFloatMessageDto dto:
                MapAndDispatchFloatEvent(dto);
                break;
            case OscIntMessageDto dto:
                MapAndDispatchIntEvent(dto);
                break;
            case OscStringMessageDto dto:
                MapAndDispatchStringEvent(dto);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MapAndDispatchStringEvent(OscStringMessageDto dto)
    {
        if (dto.Address == "/avatar/change")
        {
            var avatarId = dto.Value!.Replace("/avatar/change, ", "").Replace("\"", "");
            _eventAggregator.GetEvent<AvatarChangedEventCarrier>().Publish(new AvatarChangedEvent()
            {
                AvatarId = avatarId
            });
        }
    }

    private void MapAndDispatchIntEvent(OscIntMessageDto messageDto)
    {
        if (!_mappings.TryGetValue(buildOscAddress(messageDto.Address!), out var mappingDtos)) return;

        foreach (var mappingDto in mappingDtos)
        {
            switch (ToyTypeExtensions.GetToySupplierFromToyType(mappingDto.ToyType))
            {
                case ToySupplierEnum.Lovense:
                    _lovenseCommandDispatcher.DispatchIntCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.PiShock:
                    _piShockCommandDispatcher.DispatchIntCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void MapAndDispatchFloatEvent(OscFloatMessageDto messageDto)
    {
        if (!_mappings.TryGetValue(buildOscAddress(messageDto.Address!), out var mappingDtos)) return;

        foreach (var mappingDto in mappingDtos)
        {
            switch (ToyTypeExtensions.GetToySupplierFromToyType(mappingDto.ToyType))
            {
                case ToySupplierEnum.Lovense:
                    _lovenseCommandDispatcher.DispatchFloatCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.PiShock:
                    _piShockCommandDispatcher.DispatchFloatCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void MapAndDispatchBoolEvent(OscBoolMessageDto messageDto)
    {
        if (!_mappings.TryGetValue(buildOscAddress(messageDto.Address!), out var mappingDtos)) return;

        foreach (var mappingDto in mappingDtos)
        {
            switch (ToyTypeExtensions.GetToySupplierFromToyType(mappingDto.ToyType))
            {
                case ToySupplierEnum.Lovense:
                    _lovenseCommandDispatcher.DispatchBoolCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.PiShock:
                    _piShockCommandDispatcher.DispatchBoolCommand(mappingDto, messageDto.Value);
                    break;
                case ToySupplierEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}