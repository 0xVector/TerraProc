using TerraProc.Core.Terrain;
using TerraProc.Core.Provider;
using TerraProc.Core.Tests.TestDoubles;

namespace TerraProc.Core.Tests.Provider;

public abstract class ChunkProviderContract
{
    protected abstract IChunkProvider Create(ChunkData chunkData);

    [Fact]
    public async Task GetAsync_ShouldReturnChunkData()
    {
        var expectedChunkData = ChunkData.Zero();
        var provider = Create(expectedChunkData);
        var coords = new ChunkCoords(0, 0);

        var chunkData = await provider.GetAsync(coords);

        Assert.NotNull(chunkData);
        Assert.Equal(expectedChunkData, chunkData);
    }
}

public sealed class ConcurrentChunkProviderContract : ChunkProviderContract
{
    protected override IChunkProvider Create(ChunkData chunkData)
        => new ConcurrentChunkProvider(
            new FakeTerrainGenerator(_ => chunkData), 5);
}

public sealed class CoalescingChunkProviderContract : ChunkProviderContract
{
    protected override IChunkProvider Create(ChunkData chunkData)
        => new CoalescingChunkProvider(
            new FakeChunkProvider((_, _) => chunkData));
}