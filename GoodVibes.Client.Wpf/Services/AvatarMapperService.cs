﻿using System.Collections.Generic;
using System.Linq;
using GoodVibes.Client.Common.Extensions;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.PiShock.Constants;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.Models.Abstractions;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Newtonsoft.Json;

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
                {
                    Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name,
                    ToyType = ToyTypeExtensions.GetToyTypeFromTypeString(tm.Type)
                }).ToList()
            });

        var newMappingDtos = newMappings.Select(mappingPointViewModel => new MappingDto()
        {
            OscAddress = mappingPointViewModel.OscAddress,
            ToyMappings = mappingPointViewModel.ToyMappings.Select(tm => new ToyMappingDto()
            {
                Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name,
                ToyType = ToyTypeExtensions.GetToyTypeFromTypeString(tm.Type)
            }).ToList()
        });

        _avatarMapper.ChangeMappings(oldMappingDtos, newMappingDtos);
    }

    public IEnumerable<ToyFunctionViewModel> BuildToyFunctionViewModels(IEnumerable<LovenseToy> toyList)
    {
        var toyFunctions = new List<ToyFunctionViewModel>();

        foreach (var lovenseToy in toyList)
        {
            if (lovenseToy.Status == false || !lovenseToy.Enabled)
            {
                continue;
            }
            if (lovenseToy.Function1 != LovenseCommandEnum.None)
            {
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = lovenseToy.DisplayName,
                    Function = lovenseToy.Function1.ToString(),
                    ToyId = lovenseToy.Id!,
                    Type = lovenseToy.GetType().Name
                });
            }
            if (lovenseToy.Function2 != LovenseCommandEnum.None)
            {
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = lovenseToy.DisplayName,
                    Function = lovenseToy.Function2.ToString(),
                    ToyId = lovenseToy.Id!,
                    Type = lovenseToy.GetType().Name
                });
            }
            if (lovenseToy.Function3 != LovenseCommandEnum.None)
            {
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = lovenseToy.DisplayName,
                    Function = lovenseToy.Function3.ToString(),
                    ToyId = lovenseToy.Id!,
                    Type = lovenseToy.GetType().Name
                });
            }
        }

        return toyFunctions;
    }

    public IEnumerable<ToyFunctionViewModel> BuildToyFunctionViewModels(IEnumerable<PiShockToy> toyList)
    {
        var toyFunctions = new List<ToyFunctionViewModel>();
        foreach (var piShockToy in toyList)
        {
            if (piShockToy is PiShock.Models.PiShock piShock)
            {
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockCommandEnum.Shock.ToString(),
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockCommandEnum.MiniShock.ToString(),
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockCommandEnum.Vibrate.ToString(),
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockCommandEnum.Beep.ToString(),
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockCommandEnum.Pause.ToString(),
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockMappableFunctionsConstant.Intensity,
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piShock.FriendlyName,
                    Function = PiShockMappableFunctionsConstant.Duration,
                    ToyId = piShock.ShareCode,
                    Type = piShock.GetType().Name
                });
            }

            if (piShockToy is PiShock.Models.PiVault piVault)
            {
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.Unlock.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.ClearSession.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.AddMinutes.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.AddHours.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.AddDays.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.RemoveMinutes.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.RemoveHours.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultCommandEnum.RemoveDays.ToString(),
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
                toyFunctions.Add(new ToyFunctionViewModel()
                {
                    Name = piVault.Name,
                    Function = PiVaultMappableFunctionsConstant.AmountToAddRemove,
                    ToyId = piVault.ApiKey.ToString(),
                    Type = piVault.GetType().Name
                });
            }
        }

        return toyFunctions;
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

    public GoodVibesProfileDto DeserializeAvatarMappingProfile(string json)
    {
        var successful = json.TryParseJson<GoodVibesProfileDto>(out var profile);
        return successful ? profile : null;
    }

    public string SerializeAvatarMappingProfile(GoodVibesProfileDto goodVibesProfile)
    {
        return JsonConvert.SerializeObject(goodVibesProfile, Formatting.Indented);
    }
}