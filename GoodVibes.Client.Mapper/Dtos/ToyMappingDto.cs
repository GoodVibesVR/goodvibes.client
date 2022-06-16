using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Mapper.Dtos;

public class ToyMappingDto
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public LovenseCommandEnum Function { get; set; }
}