using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using TerraProc.Core.Provider;
using TerraProc.Core.Terrain;

namespace TerraProc.Server.Controllers;

/// <summary>
/// Basic chunk controller
/// </summary>
[ApiController]
[Route("api/chunk")]
public sealed class ChunkController(IChunkProvider provider) : ControllerBase
{
    /// <summary>
    /// Get a chunk
    /// </summary>
    /// <param name="x">X coordinate of the chunk</param>
    /// <param name="y">Y coordinate of the chunk</param>
    /// <param name="ct">Cancellation token</param>
    [HttpGet]
    // [Produces("application/x-protobuf", "application/json")]
    public async Task<IActionResult> GetChunk(
        [FromQuery] int x,
        [FromQuery] int y,
        CancellationToken ct = default)
    {
        var chunk = await provider.GetAsync((ChunkCoords)(x, y), ct);

        return Ok(new
        {
            x, y,
            tileCount = chunk.TileCount,
            byteSize = chunk.ByteSize,
            heights = MemoryMarshal.AsBytes(chunk.Heights).ToArray(),
            materials = chunk.Materials.ToArray()
        });
    }
}