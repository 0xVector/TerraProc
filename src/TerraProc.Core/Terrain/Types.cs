namespace TerraProc.Core.Terrain;

/// <summary>
/// Represents a height value.
/// Internally stored as an ushort.
/// </summary>
/// <param name="Value">height</param>
public readonly record struct Height(ushort Value)
{
    public static implicit operator ushort(Height h) => h.Value;
    public static implicit operator Height(ushort v) => new(v);
    public override string ToString() => $"{nameof(Height)}({Value})";
}

/// <summary>
/// Represents tile a material type.
/// Fits into a byte.
/// </summary>
public enum Material : byte
{
    Void = 0,
    Default = 1,
    Stone = 2,
    Grass = 3,
}

/// <summary>
/// Represents a seed for random generation.
/// Internally stored as an uint.
/// </summary>
/// <param name="Value">seed</param>
public readonly record struct Seed(uint Value)
{
    public static implicit operator uint(Seed s) => s.Value;
    public static implicit operator Seed(uint v) => new(v);
    public override string ToString() => $"{nameof(Seed)}({Value})";
}