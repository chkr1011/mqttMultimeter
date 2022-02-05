namespace MQTTnetApp.Pages.PacketInspector;

public sealed class PacketViewModel
{
    public byte[]? Data { get; init; }

    public bool IsInbound { get; init; }

    public long Length { get; init; }
    public int Number { get; init; }

    public string? Type { get; init; }
}