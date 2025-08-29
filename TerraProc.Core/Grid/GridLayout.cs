namespace TerraProc.Core.Grid;

/// <summary>
/// Static class containing grid-related constants and utilities.
/// </summary>
public static class GridLayout
{
    /// <summary>
    /// The size of each chunk in tiles
    /// </summary>
    public const int ChunkSize = 64;

    /// <summary>
    /// The total number of tiles in a chunk (ChunkSize x ChunkSize)
    /// </summary>
    public const int ChunkTileCount = ChunkSize * ChunkSize;
}

/// <summary>
/// Represents the coordinates of a chunk in the grid.
/// </summary>
/// <param name="X">The X coordinate of the chunk.</param>
/// <param name="Y">The Y coordinate of the chunk.</param>
public readonly record struct ChunkCoords(int X, int Y)
{
    public TileCoords ToTileCoords() => new(X * GridLayout.ChunkSize, Y * GridLayout.ChunkSize);
    public static implicit operator TileCoords(ChunkCoords cc) => cc.ToTileCoords();
    public static implicit operator ChunkCoords(TileCoords tc) => new(tc.X / GridLayout.ChunkSize, tc.Y / GridLayout.ChunkSize);
    public static ChunkCoords operator +(ChunkCoords a, ChunkCoords b) => new(a.X + b.X, a.Y + b.Y);
    public static ChunkCoords operator -(ChunkCoords a, ChunkCoords b) => new(a.X - b.X, a.Y - b.Y);
    public (int, int) Unpack() => (X, Y);
}

/// <summary>
/// Represents the coordinates of a tile within the grid.
/// </summary>
/// <param name="X">The X coordinate of the tile.</param>
/// <param name="Y">The Y coordinate of the tile.</param>
public readonly record struct TileCoords(int X, int Y)
{
    public ChunkCoords ToChunkCoords() => new(X / GridLayout.ChunkSize, Y / GridLayout.ChunkSize);
    public static implicit operator ChunkCoords(TileCoords tc) => tc.ToChunkCoords();
    public static implicit operator TileCoords(ChunkCoords cc) => cc.ToTileCoords();
    public static TileCoords operator +(TileCoords a, TileCoords b) => new(a.X + b.X, a.Y + b.Y);
    public static TileCoords operator -(TileCoords a, TileCoords b) => new(a.X - b.X, a.Y - b.Y);
    public (int X, int Y) Unpack() => (X, Y);
    
    /// <summary>
    /// Converts the global tile coordinates to local chunk coordinates, discarding the chunk offset.
    /// </summary>
    /// <returns></returns>
    public TileCoords ToLocal() => new(X % GridLayout.ChunkSize, Y % GridLayout.ChunkSize);
    
    /// <summary>
    /// Separate the tile coordinates into local tile and chunk components.
    /// </summary>
    /// <returns>A tuple containing the local tile coordinates and the chunk coordinates.</returns>
    public (TileCoords, ChunkCoords) ToLocalAndChunk()
    {
        var (x, y) = Unpack();
        
        var (cx, lx) = Math.DivRem(x, GridLayout.ChunkSize);
        var (cy, ly) = Math.DivRem(y, GridLayout.ChunkSize);
        
        return (new TileCoords(lx, ly), new ChunkCoords(cx, cy));
    }
}