using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using TerraProc.Core.Provider;
using TerraProc.Core.Terrain;

namespace TerraProc.Server.Controllers;

/// <summary>
/// Controller for retrieving chunk data.
/// </summary>
[ApiController]
public sealed class ChunkController(IChunkProvider provider) : ControllerBase
{
    /// <summary>
    /// Get a chunk
    /// </summary>
    /// <param name="x">X coordinate of the chunk</param>
    /// <param name="y">Y coordinate of the chunk</param>
    /// <param name="ct">Cancellation token</param>
    [HttpGet("/api/chunk/{x:int}/{y:int}")]
    [Produces("application/json")]
    public async Task<IActionResult> GetChunk(int x, int y, CancellationToken ct = default)
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