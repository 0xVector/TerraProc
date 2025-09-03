using System.Runtime.InteropServices;
using Google.Protobuf;
using Grpc.Core;
using TerraProc.Core.Provider;
using TerraProc.Core.Terrain;

namespace TerraProc.Server.Services;

/// <summary>
/// Chunk gRPC service.
/// </summary>
public sealed class ChunkService(IChunkProvider provider) : Grpc.Terrain.TerrainBase
{
    /// <summary>
    /// Get chunk data by coordinates.
    /// </summary>
    /// <param name="request">Chunk coordinates.</param>
    /// <param name="context">gRPC context.</param>
    /// <returns>Chunk data.</returns>
    public override async Task<Grpc.Chunk> GetChunk(Grpc.ChunkCoords request, ServerCallContext context)
    {
        var chunk = await provider.GetAsync(new ChunkCoords(request.X, request.Y), context.CancellationToken);

        return new Grpc.Chunk
        {
            Coord = request,
            Heights = ByteString.CopyFrom(MemoryMarshal.AsBytes(chunk.Heights)),
            Materials = ByteString.CopyFrom(MemoryMarshal.AsBytes(chunk.Materials))
        };
    }
}