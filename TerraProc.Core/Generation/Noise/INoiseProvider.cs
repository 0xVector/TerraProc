namespace TerraProc.Core.Generation.Noise;

public interface INoiseProvider
{
    /// <summary>
    /// Generates a noise value in the range of 0.0 to 1.0 for the given coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>A noise value between 0.0 and 1.0</returns>
    public double GetNoise(double x, double y);
}