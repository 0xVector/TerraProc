using TerraProc.Core.Grid;

namespace TerraProc.Core.Generation;

public interface ITerrainGenerator
{
    public ChunkData Generate(ChunkCoords coords);
}