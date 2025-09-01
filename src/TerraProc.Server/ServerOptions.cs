namespace TerraProc.Server;

public sealed class ServerOptions
{
    public int Port { get; init; } = 5001;
    public uint Seed { get; init; } = 0;
    public int Threads { get; init; } = Environment.ProcessorCount;
}