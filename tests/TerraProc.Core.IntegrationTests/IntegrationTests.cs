using TerraProc.Core.Terrain;
using TerraProc.Core.Provider;

namespace TerraProc.Core.IntegrationTests;

public class IntegrationTests
{
    public static TheoryData<ProviderConfig> Cases()
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

    [Theory]
    [MemberData(nameof(Cases))]
    public void CreateProvider_WithConfig_DoesNotThrow(ProviderConfig config)
    {
        var provider = ProviderFactory.CreateProvider(config);
        Assert.NotNull(provider);
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task Provider_GetAsync_DoesNotThrow(ProviderConfig config)
    {
        var provider = ProviderFactory.CreateProvider(config);
        var chunk = await provider.GetAsync((ChunkCoords)(0, 0));
        Assert.NotNull(chunk);
    }
}