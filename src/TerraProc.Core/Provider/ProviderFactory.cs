using TerraProc.Core.Generation;
using TerraProc.Core.Noise;
using TerraProc.Core.Terrain;

namespace TerraProc.Core.Provider;

/// <summary>
/// Configuration for creating a chunk provider.
/// </summary>
/// <param name="Seed">Seed for the terrain generator.</param>
/// <param name="MaxThreads">Maximum number of threads for concurrent chunk generation.</param>
/// <param name="UseCoalescing">Whether to use coalescing for chunk requests.</param>
public readonly record struct ProviderConfig(Seed Seed, int MaxThreads = 1, bool UseCoalescing = true);

/// <summary>
/// Provides factory methods for creating chunk providers without building them manually.
/// </summary>
public static class ProviderFactory
{
    /// <summary>
    /// Creates a default chunk provider.
    /// Uses a <see cref="NoiseTerrainGenerator"/> with <see cref="ValueNoise"/> for terrain generation,
    /// wrapped in a <see cref="ConcurrentChunkProvider"/> for multithreaded chunk generation,
    /// with a <see cref="CoalescingChunkProvider"/> to optimize chunk requests.
    /// </summary>
    /// <param name="seed">Seed for the noise generator.</param>
    /// <param name="maxThreads">Maximum number of threads for the ConcurrentChunkProvider.</param>
    /// <returns>Default configured chunk provider.</returns>
    public static IChunkProvider CreateDefaultProvider(Seed seed, int maxThreads)
    {
        return CreateProvider(new ProviderConfig(seed, maxThreads, true));
    }

    /// <summary>
    /// Creates a chunk provider based on the provided configuration.
    /// </summary>
    /// <param name="config">Configuration for the chunk provider.</param>
    /// <returns>Configured chunk provider.</returns>
    public static IChunkProvider CreateProvider(ProviderConfig config)
    {
        INoiseProvider NoiseFactory(Seed s) => new PerlinNoise(s);
        var generator = new NoiseTerrainGenerator(NoiseFactory, config.Seed);
        IChunkProvider provider = new ConcurrentChunkProvider(generator, config.MaxThreads);
        if (config.UseCoalescing)
        {
            provider = new CoalescingChunkProvider(provider);
        }

        return provider;
    }
}