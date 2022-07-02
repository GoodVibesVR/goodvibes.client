using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.Dtos.Abstractions;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using Prism.Events;

namespace GoodVibes.Client.Mapper;

public class AvatarMapperClient
{
    private readonly IEventAggregator _eventAggregator;

    private readonly Dictionary<string, List<ToyMappingDto>> _mappings;

    public AvatarMapperClient(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
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

        //foreach (var oldMapping in oldMappings)
        //{
        //    var oscAddress = buildOscAddress(oldMapping.OscAddress!);
        //    _mappings.Remove(oscAddress);
        //}

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
                MapAndPublishBoolEvent(dto);
                break;
            case OscFloatMessageDto dto:
                MapAndPublishFloatEvent(dto);
                break;
            case OscIntMessageDto dto:
                MapAndPublishIntEvent(dto);
                break;
            case OscStringMessageDto dto:
                MapAndPublishStringEvent(dto);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MapAndPublishStringEvent(OscStringMessageDto dto)
    {
        if (dto.Address == "/avatar/change")
        {
            var avatarId = dto.Value!.ToString().Replace("/avatar/change, ", "").Replace("\"", "");
            _eventAggregator.GetEvent<AvatarChangedEventCarrier>().Publish(new AvatarChangedEvent()
            {
                AvatarId = avatarId
            });
        }
    }

    private void MapAndPublishIntEvent(OscIntMessageDto dto)
    {
    }

    private void MapAndPublishFloatEvent(OscFloatMessageDto messageDto)
    {
        if (!_mappings.TryGetValue(buildOscAddress(messageDto.Address!), out var mappingDtos)) return;

        foreach (var mappingDto in mappingDtos)
        {
            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Publish(new LovenseCommandEvent
            {
                Command = mappingDto.Function,
                Toy = mappingDto.Id,
                Value = messageDto.Value
            });
        }
    }

    private void MapAndPublishBoolEvent(OscBoolMessageDto message)
    {

    }
}