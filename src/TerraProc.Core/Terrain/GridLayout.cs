namespace TerraProc.Core.Terrain;

/// <summary>
/// Static class containing grid-related constants and utilities.
/// </summary>
public static class GridLayout
{
    /// <summary>
    /// The size of each chunk in tiles.
    /// </summary>
    public const int ChunkSize = 64;

    /// <summary>
    /// The total number of tiles in a chunk (<see cref="ChunkSize"/>^2).
    /// </summary>
    public const int ChunkTileCount = ChunkSize * ChunkSize;
}