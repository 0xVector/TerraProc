using TerraProc.Core.Grid;

namespace TerraProc.Core.Generation;

public interface ITerrainGenerator
{
    public Chunk GetChunk(ChunkCoords coords);
}