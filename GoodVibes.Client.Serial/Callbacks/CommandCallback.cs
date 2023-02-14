namespace GoodVibes.Client.Serial.Callbacks;

internal class CommandCallback
{
    public int Id { get; set; }
    public string Mac { get; set; }
    public DateTime At { get; set; }
    public int Status { get; set; }
    public int Type { get; set; }
    public ValuesCallback Values { get; set; } = null!;
}