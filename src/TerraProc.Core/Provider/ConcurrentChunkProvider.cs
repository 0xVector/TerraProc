using TerraProc.Core.Generation;
using TerraProc.Core.Grid;

namespace TerraProc.Core.Provider;

/// <summary>
/// A chunk provider that generates chunks concurrently using a semaphore to limit the number of concurrent generation tasks.
/// </summary>
/// <param name="generator">The terrain generator to use for generating chunks.</param>
/// <param name="maxThreads">The maximum number of concurrent generation tasks.</param>
public sealed class ConcurrentChunkProvider(ITerrainGenerator generator, int maxThreads) : IChunkProvider
{
    private readonly SemaphoreSlim _semaphore = new(maxThreads, maxThreads);

    public async Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct).ConfigureAwait(false); // Wait for free resources
        
        try
        {
            return generator.Generate(coords);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}