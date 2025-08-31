using TerraProc.Core.Grid;

namespace TerraProc.Core.Tests.Grid;

public class ChunkDataTests
{
    [Fact]
    public void FromOwned_NullHeights_Throws()
    {
        var materials = new Material[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromOwned(null!, materials));
    }

    [Fact]
    public void FromOwned_NullMaterials_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromOwned(heights, null!));
    }

    [Fact]
    public void FromOwned_WrongHeightsLength_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount - 1];
        var materials = new Material[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromOwned(heights, materials));
    }

    [Fact]
    public void FromOwned_WrongMaterialsLength_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount - 1];
        Assert.Throws<ArgumentException>(() => ChunkData.FromOwned(heights, materials));
    }
    
    [Fact]
    public void FromOwned_ValidInputs_CreatesInstance()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];
        
        var chunkData = ChunkData.FromOwned(heights, materials);
        
        Assert.NotNull(chunkData);
        Assert.Equal(heights, chunkData.Heights.ToArray());
        Assert.Equal(materials, chunkData.Materials.ToArray());
    }

    [Fact]
    public void FromOwned_ModifyingOriginalArrays_ReflectsInChunkData()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];
        
        var chunkData = ChunkData.FromOwned(heights, materials);
        heights[0] = 5;
        materials[5] = Material.Default;
        
        Assert.Equal(5, chunkData.Heights[0]);
        Assert.Equal(Material.Default, chunkData.Materials[5]);
    }

    [Fact]
    public void FromSpan_NullHeights_Throws()
    {
        var materials = new Material[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromSpan(null!, materials));
    }

    [Fact]
    public void FromSpan_NullMaterials_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromSpan(heights, null!));
    }

    [Fact]
    public void FromSpan_WrongHeightsLength_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount - 1];
        var materials = new Material[GridLayout.ChunkTileCount];
        Assert.Throws<ArgumentException>(() => ChunkData.FromSpan(heights, materials));
    }

    [Fact]
    public void FromSpan_WrongMaterialsLength_Throws()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount - 1];
        Assert.Throws<ArgumentException>(() => ChunkData.FromSpan(heights, materials));
    }

    [Fact]
    public void FromSpan_ValidInputs_CreatesInstance()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];
        
        var chunkData = ChunkData.FromSpan(heights, materials);
        
        Assert.NotNull(chunkData);
        Assert.Equal(heights, chunkData.Heights.ToArray());
        Assert.Equal(materials, chunkData.Materials.ToArray());
    }

    [Fact]
    public void FromSpan_ModifyingOriginalArrays_DoesNotReflectInChunkData()
    {
        var heights = new ushort[GridLayout.ChunkTileCount];
        var materials = new Material[GridLayout.ChunkTileCount];

        var chunkData = ChunkData.FromSpan(heights, materials);
        heights[0] = 5;
        materials[5] = Material.Default;

        Assert.NotEqual(5, chunkData.Heights[0]);
        Assert.NotEqual(Material.Default, chunkData.Materials[5]);
    }
}