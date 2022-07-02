using System.Collections.Generic;
using System.Linq;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;
using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services;

public class AvatarMapperService : IAvatarMapperService
{
    private readonly AvatarMapperClient _avatarMapper;

    public AvatarMapperService(AvatarMapperClient avatarMapper)
    {
        _avatarMapper = avatarMapper;
    }

    public void ChangeMappings(IReadOnlyCollection<MappingPointViewModel> oldMappings, 
        IEnumerable<MappingPointViewModel> newMappings)
    {
        var oldMappingDtos = oldMappings == null
            ? new List<MappingDto>()
            : oldMappings.Select(mappingPointViewModel => new MappingDto()
            {
                OscAddress = mappingPointViewModel.OscAddress,
                ToyMappings = mappingPointViewModel.ToyMappings.Select(tm => new ToyMappingDto()
                    { Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name }).ToList()
            });

        var newMappingDtos = newMappings.Select(mappingPointViewModel => new MappingDto()
        {
            OscAddress = mappingPointViewModel.OscAddress,
            ToyMappings = mappingPointViewModel.ToyMappings.Select(tm => new ToyMappingDto()
                { Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name }).ToList()
        });

        _avatarMapper.ChangeMappings(oldMappingDtos, newMappingDtos);
    }

    public void RemoveMappingPoint(string oscAddress)
    {
        _avatarMapper.RemoveMapping(oscAddress);
    }

    public void AddMapping(string oscAddress, ToyMappingDto toyMapping)
    {
        _avatarMapper.AddMapping(oscAddress, toyMapping);
    }

    public void RemoveMapping(string oscAddress, ToyMappingDto toyMapping)
    {
        _avatarMapper.RemoveMapping(oscAddress, toyMapping);
    }

    public void ChangeOrAddMappingAddress(string oldAddress, string newAddress)
    {
        _avatarMapper.ChangeOrAddMappingAddress(oldAddress, newAddress);
    }
}