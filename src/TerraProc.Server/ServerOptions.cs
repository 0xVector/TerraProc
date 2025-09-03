namespace TerraProc.Server;

/// <summary>
/// Server options
/// </summary>
public sealed class ServerOptions
{
    /// <summary>
    /// Port to listen on
    /// </summary>
    public int Port { get; init; } = 5001;
    
    /// <summary>
    /// Seed for terrain generation
    /// </summary>
    public uint Seed { get; init; } = 0;
    
    /// <summary>
    /// Number of threads to use for terrain generation
    /// </summary>
    public int Threads { get; init; } = Environment.ProcessorCount;
}