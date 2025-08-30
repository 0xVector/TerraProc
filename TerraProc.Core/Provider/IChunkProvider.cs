using TerraProc.Core.Grid;

namespace TerraProc.Core.Provider;

public interface IChunkProvider
{
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken cancellationToken = default);
}