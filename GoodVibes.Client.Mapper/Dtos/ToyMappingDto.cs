using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Mapper.Dtos;

public class ToyMappingDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName => $"{Name} / {Function}";
    public LovenseCommandEnum Function { get; set; }
    public bool IsChecked { get; set; }
}