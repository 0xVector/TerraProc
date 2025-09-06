namespace TerraProc.Core.Terrain;

/// <summary>
/// Represents the coordinates of a chunk in the grid.
/// </summary>
/// <param name="X">X coordinate of the chunk.</param>
/// <param name="Y">Y coordinate of the chunk.</param>
public readonly record struct ChunkCoords(int X, int Y)
{
    /// <summary>
    /// Implicit conversion to tuple.
    /// </summary>
    /// <param name="cc">Chunk coordinates.</param>
    /// <returns>Tuple containing the X and Y coordinates.</returns>
    public static implicit operator (int x, int y)(ChunkCoords cc) => (cc.X, cc.Y);
    
    /// <summary>
    /// Explicit conversion from tuple.
    /// </summary>
    /// <param name="tuple">Tuple containing the X and Y coordinates.</param>
    /// <returns>Chunk coordinates.</returns>
    public static explicit operator ChunkCoords((int x, int y) tuple) => new(tuple.x, tuple.y);
    
    /// <summary>
    /// Addition operator for chunk coordinates.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Sum of the two chunk coordinates.</returns>
    public static ChunkCoords operator +(ChunkCoords a, ChunkCoords b) => new(a.X + b.X, a.Y + b.Y);
    
    /// <summary>
    /// Subtraction operator for chunk coordinates.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">>Second operand.</param>
    /// <returns>Difference of the two chunk coordinates.</returns>
    public static ChunkCoords operator -(ChunkCoords a, ChunkCoords b) => new(a.X - b.X, a.Y - b.Y);
    
    /// <summary>
    /// String representation of the chunk coordinates.
    /// </summary>
    /// <returns>String in the format "ChunkCoords(X, Y)".</returns>
    public override string ToString() => $"{nameof(ChunkCoords)}({X}, {Y})";

    /// <summary>
    /// Deconstructs the chunk coordinates into individual X and Y components.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Converts the chunk coordinates to global tile coordinates by multiplying with <see cref="GridLayout.ChunkSize"/>.
    /// </summary>
    /// <returns>Corresponding global tile coordinates.</returns>
    public TileCoords ToTileCoords() => new(X * GridLayout.ChunkSize, Y * GridLayout.ChunkSize);
}

/// <summary>
/// Represents the coordinates of a tile within the grid.
/// </summary>
/// <param name="X">X coordinate of the tile.</param>
/// <param name="Y">Y coordinate of the tile.</param>
public readonly record struct TileCoords(int X, int Y)
{
    /// <summary>
    /// Implicit conversion to tuple.
    /// </summary>
    /// <param name="tc">Tile coordinates.</param>
    /// <returns>Tuple containing the X and Y coordinates.</returns>
    public static implicit operator (int x, int y)(TileCoords tc) => (tc.X, tc.Y);
    
    /// <summary>
    /// Explicit conversion from tuple.
    /// </summary>
    /// <param name="tuple">Tuple containing the X and Y coordinates.</param>
    /// <returns>Tile coordinates.</returns>
    public static explicit operator TileCoords((int x, int y) tuple) => new(tuple.x, tuple.y);
    
    /// <summary>
    /// Addition operator for tile coordinates.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Sum of the two tile coordinates.</returns>
    public static TileCoords operator +(TileCoords a, TileCoords b) => new(a.X + b.X, a.Y + b.Y);
    
    /// <summary>
    /// Subtraction operator for tile coordinates.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <returns>Difference of the two tile coordinates.</returns>
    public static TileCoords operator -(TileCoords a, TileCoords b) => new(a.X - b.X, a.Y - b.Y);
    
    /// <summary>
    /// String representation of the tile coordinates.
    /// </summary>
    /// <returns>String in the format "TileCoords(X, Y)".</returns>
    public override string ToString() => $"{nameof(TileCoords)}({X}, {Y})";

    /// <summary>
    /// Deconstructs the tile coordinates into individual X and Y components.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Converts the global tile coordinates to chunk coordinates by dividing by <see cref="GridLayout.ChunkSize"/>,
    /// discarding the local (within-chunk) offset.
    /// </summary>
    /// <returns>Corresponding chunk coordinates.</returns>
    public ChunkCoords ToChunkCoords() => new(X / GridLayout.ChunkSize, Y / GridLayout.ChunkSize);

    /// <summary>
    /// Converts the global tile coordinates to local chunk coordinates by taking the modulus with <see cref="GridLayout.ChunkSize"/>,
    /// discarding the chunk offset.
    /// </summary>
    /// <returns>Local tile coordinates within the chunk.</returns>
    public TileCoords ToLocal() => new(X % GridLayout.ChunkSize, Y % GridLayout.ChunkSize);

    /// <summary>
    /// Separate the tile coordinates into local tile and chunk components.
    /// Uses <see cref="GridLayout.ChunkSize"/> to determine the chunk size.
    /// </summary>
    /// <returns>Tuple containing the local tile coordinates and the chunk coordinates.</returns>
    public (TileCoords, ChunkCoords) ToLocalAndChunk()
    {
        var (cx, lx) = Math.DivRem(X, GridLayout.ChunkSize);
        var (cy, ly) = Math.DivRem(Y, GridLayout.ChunkSize);
        return (new TileCoords(lx, ly), new ChunkCoords(cx, cy));
    }
}