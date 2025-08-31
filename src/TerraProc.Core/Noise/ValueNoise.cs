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
        var xi = (int)Math.Floor(x); // Obtains lattice integer point
        var yi = (int)Math.Floor(y);
        var h = (xi * 374761393) ^ (yi * 668265263) ^ seed; // Simple hash
        return (h & 0x7fffffff) / (double)int.MaxValue; // Clip sign bit, convert to [0, 1) range
    }
}