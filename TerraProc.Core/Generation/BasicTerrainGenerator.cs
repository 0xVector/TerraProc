using TerraProc.Core.Noise;
using TerraProc.Core.Grid;

namespace TerraProc.Core.Generation;

public class BasicTerrainGenerator(NoiseProviderFactory noiseFactory, int seed) : ITerrainGenerator
{
    private const ushort Scale = 10;
    private readonly INoiseProvider _noise = noiseFactory(seed);
    
    public ChunkData Generate(ChunkCoords coords)
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];

        var (globX, globY) = coords.Unpack();

        for (var i = 0; i < GridLayout.ChunkTileCount; i++)
        {
            var tileX = globX + i % GridLayout.ChunkSize;
            var tileY = globY + i / GridLayout.ChunkSize;
            var n = _noise.Sample(tileX, tileY);
            heights[i] = (ushort)(n * Scale);
            materials[i] = Material.Void;
        }
        
        return ChunkData.FromOwned(heights, materials);
    }
}