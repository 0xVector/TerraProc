using TerraProc.Core.Terrain;
using TerraProc.Core.Provider;

namespace TerraProc.Core.Tests.TestDoubles;

public sealed class FakeChunkProvider(Func<ChunkCoords, CancellationToken, ChunkData> fn) : IChunkProvider
{
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken ct = default) =>
        Task.FromResult(fn(coords, ct));
}

// A chunk provider decorator that blocks on GetAsync until Release is called.
// Useful for testing coalescing and concurrency in integration tests.
public sealed class BlockingChunkProvider(IChunkProvider provider) : IChunkProvider
{
    private readonly TaskCompletionSource _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly TaskCompletionSource _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
    public Task Entered => _entered.Task; // Task that completes when GetAsync is entered
    public void Release() => _release.TrySetResult();

    public async Task<ChunkData> GetAsync(ChunkCoords c, CancellationToken ct = default)
    {
        _entered.TrySetResult(); // Signal entered
        await _release.Task.WaitAsync(ct).ConfigureAwait(false); // Wait until released
        return await provider.GetAsync(c, ct).ConfigureAwait(false);
    }
}