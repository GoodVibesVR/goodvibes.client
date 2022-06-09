using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.Dtos.Abstractions;
using Prism.Events;

namespace GoodVibes.Client.Mapper;

public class AvatarMapper
{
    private readonly IEventAggregator _eventAggregator;

    private Dictionary<string, IList<ToyMappingDto>> _mappings;
    // private List<ToyDto> _toys; //TODO move to viewmodel


    public AvatarMapper(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        _mappings = new Dictionary<string, IList<ToyMappingDto>>();
        // _toys = new List<ToyDto>();
    }

    public void Subscribe()
    {
        // _eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(Handle);
    }

    // private void Handle(LovenseToyListUpdatedEvent obj)
    // {
    //     _toys = obj.ToyList!.Select(toy => new ToyDto
    //         {
    //             Id = toy.Id,
    //             DisplayName = toy.DisplayName!,
    //             Function1 = toy.Function1,
    //             Function2 = toy.Function2
    //         }).OrderBy(x => x.DisplayName)
    //         .ToList();
    // }

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
        // TODO Avatar changed
        // "/avatar/change"
    }

    private void MapAndPublishIntEvent(OscIntMessageDto dto)
    {
    }

    private void MapAndPublishFloatEvent(OscFloatMessageDto messageDto)
    {
        if (!_mappings.TryGetValue(messageDto.Address, out var mappingDtos)) return;

        foreach (var mappingDto in mappingDtos)
        {
            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Publish(new LovenseCommandEvent
            {
                Command = mappingDto.Function,
                Toy = mappingDto.ToyId,
                Value = messageDto.Value
            });
        }
    }

    private void MapAndPublishBoolEvent(OscBoolMessageDto message)
    {
        
    }
}