using TerraProc.Core.Grid;

namespace TerraProc.Core.Generation;

/// <summary>
/// Represents a terrain generator that can produce chunk data for given chunk coordinates.
/// </summary>
public interface ITerrainGenerator
{
    /// <summary>
    /// Generates terrain data for the specified chunk coordinates, synchronously.
    /// </summary>
    /// <param name="coords">The coordinates of the chunk to generate.</param>
    /// <returns>The generated chunk data instance.</returns>
    public ChunkData Generate(ChunkCoords coords);
}