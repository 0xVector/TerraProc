using TerraProc.Core.Generation;
using TerraProc.Core.Terrain;
using TerraProc.Core.Tests.TestDoubles;

namespace TerraProc.Core.Tests.Generation;

public abstract class TerrainGeneratorContract
{
    protected abstract ITerrainGenerator Create(Func<double, double, double> fn);
    
    [Fact]
    public void Generate_Correct_Chunk()
    {
        var generator = Create((_, _) => 0);
        var chunk = generator.Generate((ChunkCoords)(0, 0));
        
        Assert.Equal(GridLayout.ChunkTileCount, chunk.Heights.Length);
        Assert.Equal(GridLayout.ChunkTileCount, chunk.Materials.Length);
    }
}

public sealed class NoiseTerrainGeneratorContract() : TerrainGeneratorContract
{
    protected override ITerrainGenerator Create(Func<double, double, double> fn)
        => new NoiseTerrainGenerator(_ => new FakeNoiseProvider(fn), 0);
}