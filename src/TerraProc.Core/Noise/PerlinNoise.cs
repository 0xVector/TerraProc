using TerraProc.Core.Terrain;

namespace TerraProc.Core.Noise;

/// <summary>
/// Perlin noise implementation.
/// </summary>
/// <param name="seed">The seed for the noise generation.</param>
public class PerlinNoise(Seed seed) : INoiseProvider
{
    public double Sample(double x, double y)
    {
        // Integer lattice coords
        var (x0, y0) = ((int)x, (int)y);
        var (x1, y1) = (x0 + 1, y0 + 1);

        // Coords within unit cube, faded
        var xf = Fade(x - x0);
        var yf = Fade(y - y0);

        // Gradient vectors
        var g0 = GradientVec(x0, y0);
        var g1 = GradientVec(x1, y0);
        var g2 = GradientVec(x0, y1);
        var g3 = GradientVec(x1, y1);

        // Distance vectors
        var d0 = (xf - x0, yf - y0);
        var d1 = (xf - x1, yf - y0);
        var d2 = (xf - x0, yf - y1);
        var d3 = (xf - x1, yf - y1);

        // Result influences (dot products)
        var i0 = g0.Item1 * d0.Item1 + g0.Item2 * d0.Item2;
        var i1 = g1.Item1 * d1.Item1 + g1.Item2 * d1.Item2;
        var i2 = g2.Item1 * d2.Item1 + g2.Item2 * d2.Item2;
        var i3 = g3.Item1 * d3.Item1 + g3.Item2 * d3.Item2;

        // Lerp
        var a1 = Lerp(i0, i1, xf);
        var a2 = Lerp(i2, i3, xf);
        var a3 = Lerp(a1, a2, yf);

        return (a3 + 1) / 2; // Normalize to [0, 1]
    }
    
    // Hash function to get a pseudo-random value based on lattice coordinates, using the seed
    uint Hash(int x, int y)
    {
        ulong h = seed;
        h ^= (uint)x * 0x9E3779B185EBCA87UL;
        h ^= (uint)y * 0xC2B2AE3D27D4EB4FUL;
        h ^= h >> 33; h *= 0xff51afd7ed558ccdUL;
        h ^= h >> 33; h *= 0xc4ceb9fe1a85ec53UL;
        h ^= h >> 33;
        return (uint)h;
    }

    // Deterministic gradient vector based on lattice coordinates, from a fixed set
    private (int, int) GradientVec(int x, int y)
    {
        return (Hash(x, y) & 0x7) switch
        {
            0x0 => (1, 1),
            0x1 => (-1, 1),
            0x2 => (1, -1),
            0x3 => (-1, -1),
            0x4 => (1, 0),
            0x5 => (-1, 0),
            0x6 => (0, 1),
            0x7 => (0, -1),
            _ => (0, 0)
        };
        // (int, int)[] vectors = [(1, 1), (-1, 1), (1, -1), (-1, -1), (1, 0), (-1, 0), (0, 1), (0, -1)];
        // return vectors[hash & 0x7];
    }

    // Linear interpolation
    private static double Lerp(double a, double b, double x) => a + x * (b - a);

    // Fade function to ease coordinate values
    private static double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);
}