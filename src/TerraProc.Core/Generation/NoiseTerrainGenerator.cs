using TerraProc.Core.Noise;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Generation;

/// <summary>
/// A basic terrain generator.
/// </summary>
/// <param name="noiseFactory">A factory function to create a noise provider given a seed.</param>
/// <param name="seed">The main seed for the terrain generation.</param>
public class NoiseTerrainGenerator(NoiseProviderFactory noiseFactory, Seed seed) : ITerrainGenerator
{
    private readonly INoiseProvider _noise = noiseFactory(seed);
    
    public ChunkData Generate(ChunkCoords coords)
    {
        var heights = new Height[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];

        var (globX, globY) = coords;

        for (var i = 0; i < GridLayout.ChunkTileCount; i++)
        {
            var tileX = globX + i % GridLayout.ChunkSize;
            var tileY = globY + i / GridLayout.ChunkSize;
            var n = _noise.Sample(tileX, tileY);
            heights[i] = (Height)(n * GridLayout.MaxHeight);
            materials[i] = Material.Default;
        }
        
        return ChunkData.FromOwned(heights, materials);
    }
}