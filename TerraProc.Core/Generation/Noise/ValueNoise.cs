namespace TerraProc.Core.Generation.Noise;

public class ValueNoise(int seed) : INoiseProvider
{
    public double GetNoise(double x, double y)
    {
        var xi = (int)Math.Floor(x); // Obtains lattice integer point
        var yi = (int)Math.Floor(y);
        var h = (xi * 374761393) ^ (yi * 668265263) ^ seed; // Simple hash
        return (h & 0x7fffffff) / (double)int.MaxValue; // Clip sign bit, convert to [0, 1) range
    }
}