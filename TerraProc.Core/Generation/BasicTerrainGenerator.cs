using TerraProc.Core.Generation.Noise;
using TerraProc.Core.Grid;

namespace TerraProc.Core.Generation;

public class BasicTerrainGenerator(INoiseProvider noise) : ITerrainGenerator
{
    private const ushort Scale = 10;
    
    public Chunk GetChunk(ChunkCoords coords)
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];

        var (globX, globY) = coords.Unpack();

        for (var i = 0; i < GridLayout.ChunkTileCount; i++)
        {
            var tileX = globX + i % GridLayout.ChunkSize;
            var tileY = globY + i / GridLayout.ChunkSize;
            var n = noise.GetNoise(tileX, tileY);
            heights[i] = (ushort)(n * Scale);
            materials[i] = Material.Void;
        }
        
        return Chunk.FromOwned(coords, heights, materials);
    }
}