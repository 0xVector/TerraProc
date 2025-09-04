using System.Runtime.InteropServices;

namespace TerraProc.Core.Terrain;

/// <summary>
/// Represents a chunk in the grid, containing its coordinates and data.
/// </summary>
/// <param name="Coords">The coordinates of the chunk.</param>
/// <param name="Data">The data of the chunk.</param>
public readonly record struct Chunk(ChunkCoords Coords, ChunkData Data)
{
    public override string ToString() => $"{nameof(Chunk)}({Coords}, {Data})";
}

/// <summary>
/// Represents a single tile in the grid, containing its height and material.
/// </summary>
/// <param name="HeightValue">Height of the tile.</param>
/// <param name="MaterialValue">Material of the tile.</param>
public readonly record struct Tile(Height HeightValue, Material MaterialValue)
{
    public override string ToString() => $"{nameof(Tile)}(Height: {HeightValue}, Material: {MaterialValue})";
}

/// <summary>
/// Represents a chunk of the grid, containing heights and materials for each tile.
/// </summary>
public sealed class ChunkData
{
    private readonly Height[] _heights;
    private readonly Material[] _materials;

    private ChunkData(Height[] heights, Material[] materials)
    {
        _heights = heights;
        _materials = materials;
    }

    /// <summary>
    /// The number of tiles in the chunk (should be equal to <see cref="GridLayout.ChunkTileCount"/>).
    /// </summary>
    public int TileCount => _heights.Length;

    /// <summary>
    /// The size of the chunk data in bytes in memory.
    /// </summary>
    /// <returns>Size in bytes.</returns>
    public int ByteSize => Marshal.SizeOf<Height>() * _heights.Length + sizeof(Material) * _materials.Length;

    public ReadOnlySpan<Height> Heights => _heights;
    public ReadOnlyMemory<Height> HeightsMemory => _heights;
    public ReadOnlySpan<Material> Materials => _materials;
    public ReadOnlyMemory<Material> MaterialsMemory => _materials;

    /// <summary>
    /// Get the <see cref="Tile"/> by the <b>local</b> coordinates within the chunk.
    /// </summary>
    /// <param name="x">The local X coordinate within the chunk (0 to <see cref="GridLayout.ChunkSize"/>-1)</param>
    /// <param name="y">The local Y coordinate within the chunk (0 to <see cref="GridLayout.ChunkSize"/>-1)</param>
    public Tile this[int x, int y]
    {
        get
        {
            ValidateLocalCoords(x, y);
            return new Tile(_heights[Linearize(x, y)], _materials[Linearize(x, y)]);
        }
        set
        {
            ValidateLocalCoords(x, y);
            _heights[Linearize(x, y)] = value.HeightValue;
            _materials[Linearize(x, y)] = value.MaterialValue;
        }
    }
    
    /// <summary>
    /// Get the <see cref="Tile"/> by the <b>global</b> tile coordinates.
    /// </summary>
    /// <param name="coords">The global tile coordinates.</param>
    public Tile this[TileCoords coords]
    {
        get
        {
            var (x, y) = coords.ToLocal();
            return this[x, y];
        }
        set
        {
            var (x, y) = coords.ToLocal();
            this[x, y] = value;
        }
    }

    /// <summary>
    /// Create a chunk from owned arrays of heights and materials.
    /// The caller promises that they will not be modified after being passed to this method.
    /// The arrays must be of length <see cref="GridLayout.ChunkTileCount"/>.
    /// </summary>
    /// <param name="heights"></param>
    /// <param name="materials"></param>
    /// <returns>The created chunk.</returns>
    public static ChunkData FromOwned(Height[] heights, Material[] materials)
    {
        ValidateSizes(heights, materials);
        return new ChunkData(heights, materials);
    }

    /// <summary>
    /// Create a chunk from spans of heights and materials.
    /// The spans will be copied into new arrays.
    /// The arrays must be of length <see cref="GridLayout.ChunkTileCount"/>.
    /// </summary>
    /// <param name="heights"></param>
    /// <param name="materials"></param>
    /// <returns>The created chunk.</returns>
    public static ChunkData FromSpan(ReadOnlySpan<Height> heights, ReadOnlySpan<Material> materials)
    {
        ValidateSizes(heights, materials);
        return new ChunkData(heights.ToArray(), materials.ToArray());
    }

    /// <summary>
    ///  Create a chunk with all heights set to zero and all materials set to <see cref="Material.Void"/>.
    /// </summary>
    /// <returns>The created chunk.</returns>
    public static ChunkData Zero()
    {
        return new ChunkData(new Height[GridLayout.ChunkTileCount], new Material[GridLayout.ChunkTileCount]);
    }

    private static void ValidateLocalCoords(int x, int y)
    {
        if (x is < 0 or >= GridLayout.ChunkSize)
            throw new ArgumentOutOfRangeException(nameof(x),
                $"X coordinate must be in range [0, {GridLayout.ChunkSize})");
        if (y is < 0 or >= GridLayout.ChunkSize)
            throw new ArgumentOutOfRangeException(nameof(y),
                $"Y coordinate must be in range [0, {GridLayout.ChunkSize})");
    }

    private static void ValidateSizes(ReadOnlySpan<Height> heights, ReadOnlySpan<Material> materials)
    {
        if (heights.Length != GridLayout.ChunkTileCount)
            throw new ArgumentException($"Heights span must be of length {GridLayout.ChunkTileCount}", nameof(heights));
        if (materials.Length != GridLayout.ChunkTileCount)
            throw new ArgumentException($"Materials span must be of length {GridLayout.ChunkTileCount}",
                nameof(materials));
    }

    private static int Linearize(int x, int y) => y * GridLayout.ChunkSize + x;
}