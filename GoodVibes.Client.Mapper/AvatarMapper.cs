using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.Dtos.Abstractions;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using Newtonsoft.Json;
using Prism.Events;

namespace GoodVibes.Client.Mapper;

public class AvatarMapper
{
    private readonly IEventAggregator _eventAggregator;

    private readonly Dictionary<string, IList<ToyMappingDto>> _mappings;
    
    public AvatarMapper(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        _mappings = new Dictionary<string, IList<ToyMappingDto>>();
    }

    public void AddMapping(string oscAddress, ToyMappingDto toyMappingDto)
    {
        if (_mappings.TryGetValue(oscAddress, out var mappingDtos))
        {
            mappingDtos.Add(toyMappingDto);
            //TODO make mapping list updated event
            
        }   
        else
        {
            _mappings.Add(oscAddress, new List<ToyMappingDto> { toyMappingDto });
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
        Console.WriteLine($"String value returned: {JsonConvert.SerializeObject(dto)}");

        // TODO Avatar changed
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
        if (!_mappings.TryGetValue(messageDto.Address!, out var mappingDtos)) return;

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