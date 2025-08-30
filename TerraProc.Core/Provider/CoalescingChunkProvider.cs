using System.Collections.Concurrent;
using TerraProc.Core.Grid;

namespace TerraProc.Core.Provider;

/// <summary>
/// A chunk provider that coalesces multiple requests for the same chunk into a single request.
/// This prevents redundant generation of the same chunk when multiple requests are made concurrently.
/// </summary>
/// <param name="provider">The underlying chunk provider to use for generating chunks.</param>
public sealed class CoalescingChunkProvider(IChunkProvider provider) : IChunkProvider
{
    private readonly ConcurrentDictionary<ChunkCoords, Task<ChunkData>> _inflight = new();

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