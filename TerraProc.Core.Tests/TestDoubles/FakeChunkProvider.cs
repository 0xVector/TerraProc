using TerraProc.Core.Grid;
using TerraProc.Core.Provider;

namespace TerraProc.Core.Tests.TestDoubles;

public sealed class FakeChunkProvider(Func<ChunkCoords, CancellationToken, ChunkData> fn) : IChunkProvider
{
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken ct = default) =>
        Task.FromResult(fn(coords, ct));
}