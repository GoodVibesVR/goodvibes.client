using System.Data.Common;
using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Mapper.Dtos;

public class ToyDto
{
    public string? Id { get; set; }
    public string DisplayName { get; set; }
    public LovenseCommandEnum Function1 { get; set; }
    public LovenseCommandEnum Function2 { get; set; }
}