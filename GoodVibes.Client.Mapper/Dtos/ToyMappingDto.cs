using GoodVibes.Client.Common.Enums;

namespace GoodVibes.Client.Mapper.Dtos;

public class ToyMappingDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName => $"{Name} / {Function}";
    public string? Function { get; set; }
    public bool IsChecked { get; set; }
    public ToyTypeEnum ToyType { get; set; }
}