using TerraProc.Core.Generation;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Provider;

/// <summary>
/// A chunk provider that generates chunks concurrently using a semaphore to limit the number of concurrent generation tasks.
/// </summary>
/// <param name="generator">Terrain generator to use for generating chunks.</param>
/// <param name="maxThreads">Maximum number of concurrent generation tasks.</param>
public sealed class ConcurrentChunkProvider(ITerrainGenerator generator, int maxThreads) : IChunkProvider
{
    private readonly SemaphoreSlim _semaphore = new(maxThreads, maxThreads);

    /// <summary>
    /// Gets the chunk data for the specified coordinates while respecting the concurrency limit.
    /// </summary>
    /// <param name="coords">Coordinates of the chunk to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>Chunk data.</returns>
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