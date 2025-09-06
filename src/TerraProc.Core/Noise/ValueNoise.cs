using TerraProc.Core.Terrain;

namespace TerraProc.Core.Noise;

/// <summary>
/// Simple value noise implementation that provides consistent pseudo-random values based on input coordinates and a seed.
/// </summary>
/// <param name="seed">Seed for the noise generation.</param>
public class ValueNoise(Seed seed) : INoiseProvider
{
    /// <summary>
    /// Sample the value noise at the given coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>Noise value in the range [0, 1).</returns>
    public double Sample(double x, double y)
    {
        return Hash(x, y);
    }
    
    // Just the hash function used in PerlinNoise implementation adopted for doubles
    private double Hash(double x, double y)
    {
        ulong h = seed;
        h ^= (uint)x * 0x9E3779B185EBCA87UL;
        h ^= (uint)y * 0xC2B2AE3D27D4EB4FUL;
        h ^= h >> 33;
        h *= 0xff51afd7ed558ccdUL;
        h ^= h >> 33;
        h *= 0xc4ceb9fe1a85ec53UL;
        h ^= h >> 33;
        return (uint)h / (double)uint.MaxValue;
    }
}