using TerraProc.Core.Grid;

namespace TerraProc.Core.Provider;

/// <summary>
/// Represents a provider that can supply chunk data for given coordinates.
/// </summary>
public interface IChunkProvider
{
    /// <summary>
    /// Asynchronously retrieves chunk data for the specified coordinates.
    /// </summary>
    /// <param name="coords">The coordinates of the chunk to retrieve.</param>
    /// <param name="ct">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the chunk data.</returns>
    public Task<ChunkData> GetAsync(ChunkCoords coords, CancellationToken ct = default);
}