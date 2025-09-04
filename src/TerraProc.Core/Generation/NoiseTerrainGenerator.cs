using TerraProc.Core.Noise;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Generation;

/// <summary>
/// Noise-based terrain generator that samples a noise function, with various octave and band transformations.
/// </summary>
/// <param name="noiseFactory">Factory function to create a noise provider given a seed.</param>
/// <param name="seed">Main seed for the terrain generation.</param>
public class NoiseTerrainGenerator(NoiseProviderFactory noiseFactory, Seed seed) : ITerrainGenerator
{
    private readonly INoiseProvider _noise = noiseFactory(seed);

    public ChunkData Generate(ChunkCoords coords)
    {
        var heights = new Height[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];

        var (globX, globY) = coords.ToTileCoords();

        for (var i = 0; i < GridLayout.ChunkTileCount; i++)
        {
            var (yOff, xOff) = Math.DivRem(i, GridLayout.ChunkSize); // TODO: add (de)linearize public API 
            var tileX = globX + xOff + .5; // Center of tile
            var tileY = globY + yOff + .5;
            var n = _noise.Sample(tileX, tileY);
            heights[i] = (Height)(n * GridLayout.MaxHeight);
            materials[i] = Material.Default;
        }

        return ChunkData.FromOwned(heights, materials);
    }
}