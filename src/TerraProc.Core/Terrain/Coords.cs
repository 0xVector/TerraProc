namespace TerraProc.Core.Terrain;

/// <summary>
/// Represents the coordinates of a chunk in the grid.
/// </summary>
/// <param name="X">The X coordinate of the chunk.</param>
/// <param name="Y">The Y coordinate of the chunk.</param>
public readonly record struct ChunkCoords(int X, int Y)
{
    // public static implicit operator TileCoords(ChunkCoords cc) => cc.ToTileCoords(); // Implicit -> TileCoords
    public static implicit operator (int x, int y)(ChunkCoords cc) => (cc.X, cc.Y); // Implicit -> tuple
    public static explicit operator ChunkCoords((int x, int y) tuple) => new(tuple.x, tuple.y); // Explicit <- tuple
    public static ChunkCoords operator +(ChunkCoords a, ChunkCoords b) => new(a.X + b.X, a.Y + b.Y);
    public static ChunkCoords operator -(ChunkCoords a, ChunkCoords b) => new(a.X - b.X, a.Y - b.Y);
    public override string ToString() => $"{nameof(ChunkCoords)}({X}, {Y})";

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Converts the chunk coordinates to global tile coordinates by multiplying with <see cref="GridLayout.ChunkSize"/>.
    /// </summary>
    /// <returns>The corresponding global tile coordinates.</returns>
    public TileCoords ToTileCoords() => new(X * GridLayout.ChunkSize, Y * GridLayout.ChunkSize);
}

/// <summary>
/// Represents the coordinates of a tile within the grid.
/// </summary>
/// <param name="X">The X coordinate of the tile.</param>
/// <param name="Y">The Y coordinate of the tile.</param>
public readonly record struct TileCoords(int X, int Y)
{
    // public static implicit operator ChunkCoords(TileCoords tc) => tc.ToChunkCoords(); // Implicit -> ChunkCoords
    public static implicit operator (int x, int y)(TileCoords tc) => (tc.X, tc.Y); // Implicit -> tuple
    public static explicit operator TileCoords((int x, int y) tuple) => new(tuple.x, tuple.y); // Explicit <- tuple
    public static TileCoords operator +(TileCoords a, TileCoords b) => new(a.X + b.X, a.Y + b.Y);
    public static TileCoords operator -(TileCoords a, TileCoords b) => new(a.X - b.X, a.Y - b.Y);
    public override string ToString() => $"{nameof(TileCoords)}({X}, {Y})";

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Converts the global tile coordinates to chunk coordinates by dividing by <see cref="GridLayout.ChunkSize"/>,
    /// discarding the local (within-chunk) offset.
    /// </summary>
    /// <returns></returns>
    public ChunkCoords ToChunkCoords() => new(X / GridLayout.ChunkSize, Y / GridLayout.ChunkSize);

    /// <summary>
    /// Converts the global tile coordinates to local chunk coordinates by taking the modulus with <see cref="GridLayout.ChunkSize"/>,
    /// discarding the chunk offset.
    /// </summary>
    /// <returns></returns>
    public TileCoords ToLocal() => new(X % GridLayout.ChunkSize, Y % GridLayout.ChunkSize);

    /// <summary>
    /// Separate the tile coordinates into local tile and chunk components.
    /// Uses <see cref="GridLayout.ChunkSize"/> to determine the chunk size.
    /// </summary>
    /// <returns>A tuple containing the local tile coordinates and the chunk coordinates.</returns>
    public (TileCoords, ChunkCoords) ToLocalAndChunk()
    {
        var (cx, lx) = Math.DivRem(X, GridLayout.ChunkSize);
        var (cy, ly) = Math.DivRem(Y, GridLayout.ChunkSize);
        return (new TileCoords(lx, ly), new ChunkCoords(cx, cy));
    }
}