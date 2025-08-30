using TerraProc.Core.Generation;
using TerraProc.Core.Noise;

namespace TerraProc.Core.Provider;

public static class ProviderFactory
{
    public static IChunkProvider CreateDefaultProvider(int seed, int maxThreads)
    {
        return
            new CoalescingChunkProvider(
                new ConcurrentChunkProvider(
                    new BasicTerrainGenerator(
                        s => new ValueNoise(s), seed),
                    maxThreads)
            );
    }
}