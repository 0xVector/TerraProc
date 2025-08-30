using TerraProc.Core.Grid;

namespace TerraProc.Core.Provider;

public class ConcurrentChunkProvider : IChunkProvider
{
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}