using System.Collections.Concurrent;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Provider;

/// <summary>
/// Chunk provider that coalesces multiple requests for the same chunk into a single request.
/// This prevents redundant generation of the same chunk when multiple requests are made concurrently.
/// </summary>
/// <param name="provider">Underlying chunk provider to use for generating chunks.</param>
public sealed class CoalescingChunkProvider(IChunkProvider provider) : IChunkProvider
{
    private readonly ConcurrentDictionary<ChunkCoords, Task<ChunkData>> _inflight = new();

     /// <summary>
    /// Gets the chunk at the specified coordinates, coalescing multiple requests for the same chunk.
    /// </summary>
    /// <param name="coords">Coordinates of the chunk to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the request.</param>
    /// <returns>Chunk data.</returns>
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken ct = default)
    {
        // Try to get an existing inflight task or create a new one if none exists
        var inflight = _inflight.GetOrAdd(coords, k =>
        {
            // Get a new task, and when it completes, remove it from the inflight dictionary
            var task = provider.GetAsync(k, CancellationToken.None);
            task.ContinueWith(_ => _inflight.TryRemove(k, out var _), TaskScheduler.Default);
            return task; // Return the provider task (not the continuation)
        });

        return inflight.WaitAsync(ct); // To support cancellation but not cancel the underlying task for other waiters
    }
}

// TODO: Stop when all waiters are cancelled