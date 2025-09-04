using TerraProc.Core.Terrain;

namespace TerraProc.Core.Noise;

/// <summary>
/// A simple value noise implementation that provides consistent pseudo-random values based on input coordinates and a seed.
/// </summary>
/// <param name="seed">The seed for the noise generation.</param>
public class ValueNoise(Seed seed) : INoiseProvider
{
    public double Sample(double x, double y)
    {
        return Hash(x, y);
        var xi = (int)Math.Floor(x); // Obtains lattice integer point
        var yi = (int)Math.Floor(y);
        long h = (xi * 374761393) ^ (yi * 668265263) ^ seed; // Simple hash
        return (h & 0x7fffffff) / (double)int.MaxValue; // Clip sign bit, convert to [0, 1) range
    }
    
    double Hash(double x, double y)
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