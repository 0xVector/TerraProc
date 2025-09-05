using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TerraProc.Core.Provider;
using TerraProc.Core.Terrain;

namespace TerraProc.Server.Controllers;

/// <summary>
/// Controller for retrieving chunk images.
/// </summary>
[ApiController]
public sealed class ChunkImageController(IChunkProvider provider) : ControllerBase
{
    /// <summary>
    /// View chunk as an image.
    /// </summary>
    /// <param name="x">X coordinate of the chunk.</param>
    /// <param name="y">Y coordinate of the chunk.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns></returns>
    [HttpGet("/view/chunk/{x:int}/{y:int}")]
    [Produces("image/webp")]
    public async Task<IActionResult> GetChunkImage(int x, int y, CancellationToken ct = default)
    {
        var chunk = await provider.GetAsync(new ChunkCoords(x, y), ct);
        Response.Headers.CacheControl = "public,max-age=1000";
        return File(RenderChunk(chunk), "image/webp");
    }
    
    // TODO: refactor to something separate
    private byte[] RenderChunk(ChunkData chunk)
    {
        using var img = new Image<Rgba32>(GridLayout.ChunkSize, GridLayout.ChunkSize);

        for (var i = 0; i < GridLayout.ChunkTileCount; i++)
        {
            var (y, x) = Math.DivRem(i, GridLayout.ChunkSize);
            img[x, y] = GetPixel(chunk.Heights[i], chunk.Materials[i]);
        }

        // Resize (TODO: something more robust)
        img.Mutate(ctx => ctx.Resize(img.Width * 8, img.Height * 8, KnownResamplers.NearestNeighbor));

        // Encode
        var memory = new MemoryStream();
        img.Save(memory, new WebpEncoder());
        return memory.ToArray();
    }

    private Rgba32 GetPixel(Height height, Material material)
    {
        var c = GetColor(material);
        var relHeight = (double)height / GridLayout.MaxHeight;
        return new Rgba32(
            // (byte)(c.R * relHeight),
            // (byte)(c.G * relHeight),
            // (byte)(c.B * relHeight)
            (byte)(c.R),
            (byte)(c.G),
            (byte)(c.B),
            (byte)(relHeight * 255)
        );
    }

    private Rgba32 GetColor(Material material)
        => material switch
        {
            Material.Void => new Rgba32(0, 0, 0),
            Material.Default => new Rgba32(35, 35, 35),
            Material.Stone => new Rgba32(75, 75, 75),
            Material.Grass => new Rgba32(30, 220, 80),
            _ => new Rgba32(20, 20, 20),
        };
}