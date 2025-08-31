using TerraProc.Core.Generation;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Tests.TestDoubles;

public sealed class FakeTerrainGenerator(Func<ChunkCoords, ChunkData> fn) : ITerrainGenerator
{
    public ChunkData Generate(ChunkCoords coords) => fn(coords);
}