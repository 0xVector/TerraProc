namespace TerraProc.Core.Terrain;

/// <summary>
/// Represents a height value.
/// Internally stored as an ushort.
/// </summary>
/// <param name="Value">height</param>
public readonly record struct Height(ushort Value)
{
    /// <summary>
    /// Implicit conversion to ushort.
    /// </summary>
    /// <param name="h">Height.</param>
    /// <returns>ushort value.</returns>
    public static implicit operator ushort(Height h) => h.Value;
    
    /// <summary>
    /// Implicit conversion from ushort.
    /// </summary>
    /// <param name="v">ushort value.</param>
    /// <returns>Height.</returns>
    public static implicit operator Height(ushort v) => new(v);
    
    /// <summary>
    /// String representation of the height.
    /// </summary>
    /// <returns>String in the format "Height(value)".</returns>
    public override string ToString() => $"{nameof(Height)}({Value})";
}

/// <summary>
/// Represents tile a material type.
/// Fits into a byte.
/// </summary>
public enum Material : byte
{
    /// <summary>
    /// No material value.
    /// </summary>
    Void = 0,
    
    /// <summary>
    /// Default material.
    /// </summary>
    Default = 1,
    
    /// <summary>
    /// Stone material.
    /// </summary>
    Stone = 2,
    
    /// <summary>
    /// Grass material.
    /// </summary>
    Grass = 3,
}

/// <summary>
/// Represents a seed for random generation.
/// Internally stored as an uint.
/// </summary>
/// <param name="Value">seed</param>
public readonly record struct Seed(uint Value)
{
    /// <summary>
    /// Implicit conversion to uint.
    /// </summary>
    /// <param name="s">Seed.</param>
    /// <returns>uint value.</returns>
    public static implicit operator uint(Seed s) => s.Value;
    
    /// <summary>
    /// Implicit conversion from uint.
    /// </summary>
    /// <param name="v">uint value.</param>
    /// <returns>Seed.</returns>
    public static implicit operator Seed(uint v) => new(v);
    
    /// <summary>
    /// String representation of the seed.
    /// </summary>
    /// <returns>String in the format "Seed(value)".</returns>
    public override string ToString() => $"{nameof(Seed)}({Value})";
}