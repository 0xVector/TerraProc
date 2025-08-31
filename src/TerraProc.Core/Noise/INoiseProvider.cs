namespace TerraProc.Core.Noise;

public interface INoiseProvider
{
    /// <summary>
    /// Generates a noise value sample in the range of 0.0 to 1.0 for the given coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>A noise value between 0.0 and 1.0</returns>
    public double Sample(double x, double y);
}

/// <summary>
/// A factory delegate for creating instances of INoiseProvider with a specified seed.
/// </summary>
public delegate INoiseProvider NoiseProviderFactory(int seed);