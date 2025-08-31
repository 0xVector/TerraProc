using TerraProc.Core.Generation;
using TerraProc.Core.Noise;
using TerraProc.Core.Terrain;
using TerraProc.Core.Provider;
using TerraProc.Core.Tests.TestDoubles;

namespace TerraProc.Core.IntegrationTests;

public class IntegrationTests
{
    public static TheoryData<ProviderConfig> Configs()
    {
        return
        [
            new ProviderConfig(0, 1, true),
            new ProviderConfig(42, 4, true),
            new ProviderConfig(7, 2, false),
            new ProviderConfig(100, 8, false),
            new ProviderConfig(uint.MaxValue, 16, true),
            new ProviderConfig(uint.MaxValue, 32, false)
        ];
    }

    public static TheoryData<ChunkCoords> Coords()
    {
        return
        [
            (ChunkCoords)(0, 0),
            (ChunkCoords)(1, 1),
            (ChunkCoords)(-1, -1),
            (ChunkCoords)(100, 100),
            (ChunkCoords)(-100, -100)
        ];
    }

    public static TheoryData<ProviderConfig> CoalescingConfigs() =>
        new(Configs().Where<ProviderConfig>(c => c.UseCoalescing));

    public static TheoryData<ProviderConfig, ChunkCoords> ConfigsWithCoords() => Combine(Configs(), Coords());

    public static TheoryData<ProviderConfig, ChunkCoords> CoalescingConfigsWithCoords() =>
        Combine(CoalescingConfigs(), Coords());

    [Theory]
    [MemberData(nameof(Configs))]
    public void CreateProvider_WithConfig_DoesNotThrow(ProviderConfig config)
    {
        var provider = ProviderFactory.CreateProvider(config);
        Assert.NotNull(provider);
    }

    [Theory]
    [MemberData(nameof(ConfigsWithCoords))]
    public async Task Provider_GetAsync_DoesNotThrow(ProviderConfig config, ChunkCoords c)
    {
        var provider = ProviderFactory.CreateProvider(config);
        var chunk = await provider.GetAsync(c);
        Assert.NotNull(chunk);
    }

    [Theory]
    [MemberData(nameof(ConfigsWithCoords))]
    public async Task Provider_GetAsync_SameChunkMultipleTimes_ReturnsSameData(ProviderConfig config, ChunkCoords c)
    {
        var provider = ProviderFactory.CreateProvider(config);
        var chunk1 = await provider.GetAsync(c);
        var chunk2 = await provider.GetAsync(c);
        Assert.Equal(chunk1.Heights.ToArray(), chunk2.Heights.ToArray());
        Assert.Equal(chunk1.Materials.ToArray(), chunk2.Materials.ToArray());
    }

    [Theory]
    [MemberData(nameof(CoalescingConfigsWithCoords))]
    public async Task CoalescingProvider_GetAsync_ConcurrentRequests_SameChunk_ReturnsSameIdentity(
        ProviderConfig config, ChunkCoords c)
    {
        // TODO: Add some plumbing so we don't have to recreate the provider here

        // Shared blocking generator to ensure both requests block until we allow them to proceed, forcing coalescing
        var blocker = new BlockingChunkProvider(
            new ConcurrentChunkProvider(
                new NoiseTerrainGenerator(s => new ValueNoise(s), config.Seed), 5));
        // Insert a block between the coalescing provider and the real one
        var provider = new CoalescingChunkProvider(blocker);

        var chunk1Task = provider.GetAsync(c);
        var chunk2Task = provider.GetAsync(c);

        await blocker.Entered;
        blocker.Release(); // Allow both requests to proceed
        var chunk1 = await chunk1Task;
        var chunk2 = await chunk2Task;

        Assert.Same(chunk1, chunk2); // Reference equality
    }

    // Helper method to create Cartesian product of two TheoryData sets
    private static TheoryData<T1, T2> Combine<T1, T2>(TheoryData<T1> data1, TheoryData<T2> data2)
    {
        var data = new TheoryData<T1, T2>();
        foreach (var item1 in data1)
        {
            foreach (var item2 in data2)
            {
                data.Add(item1, item2);
            }
        }

        return data;
    }
}