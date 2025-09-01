using Microsoft.AspNetCore.Mvc;
using TerraProc.Core.Provider;
using TerraProc.Core.Terrain;

namespace TerraProc.Server.Controllers;

/// <summary>
/// Basic chunk controller
/// </summary>
[ApiController]
[Route("api/chunks")]
public sealed class ChunksController(IChunkProvider provider) : ControllerBase
{
    /// <summary>
    /// Get a chunk
    /// </summary>
    /// <param name="x">X coordinate of the chunk</param>
    /// <param name="y">Y coordinate of the chunk</param>
    /// <param name="ct">Cancellation token</param>
    [HttpGet]
    public async Task<IActionResult> GetChunk(
        [FromQuery] int x,
        [FromQuery] int y,
        CancellationToken ct = default)
    {
        var chunk = await provider.GetAsync((ChunkCoords)(x, y), ct);

        // TODO: something better than this
        var heightsAsPrimitive = new ushort[chunk.Heights.Length];
        foreach (var h in chunk.Heights) heightsAsPrimitive[h.Value] = h;

        return Ok(new
        {
            x, y,
            tileCount = chunk.TileCount,
            byteSize = chunk.ByteSize,
            heights = heightsAsPrimitive,
            materials = chunk.Materials.ToArray()
        });
    }
}