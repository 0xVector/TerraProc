namespace TerraProc.Core.Noise;

/// <summary>
/// Extension methods for INoiseProvider to support advanced noise sampling techniques.
/// </summary>
public static class NoiseProviderExtensions

{
    public static double SampleBand(this INoiseProvider provider, double x, double y, double freq, double amp)
        => provider.Sample(x * freq, y * freq) * amp;


    /// <summary>
    /// Compute a sum of samples at a number of octaves.
    /// </summary>
    /// <param name="provider">Noise provider.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="octaves">Number of octaves to sample.</param>
    /// <param name="persistence">Persistence (amplitude multiplier) per octave.</param>
    /// <returns>Normalized noise value in the range [0, 1].</returns>
    public static double SampleOctaves(this INoiseProvider provider, double x, double y, int octaves,
        double persistence)
    {
        double total = 0;
        double freq = 1;
        double amp = 1;
        double maxVal = 0; // For normalization of the result

        for (var i = 0; i < octaves; i++)
        {
            total += provider.SampleBand(x, y, freq, amp);

            maxVal += amp;
            amp *= persistence;
            freq *= 2;
        }

        return total / maxVal;
    }
}