using System.Collections.Generic;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IAvatarMapperService
{
    void ChangeMappings(IReadOnlyCollection<MappingPointViewModel> oldMappings,
        IEnumerable<MappingPointViewModel> newMappings);
    
    void RemoveMappingPoint(string oscAddress);

    void AddMapping(string oscAddress, ToyMappingDto toyMapping);

    void RemoveMapping(string oscAddress, ToyMappingDto toyMapping);

    void ChangeOrAddMappingAddress(string oldAddress, string newAddress);
}