using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Net.Http;
using Grpc.Net.Client;

namespace GrpcClient
{
    /// <summary>
    /// Client for interacting with the ChunkService gRPC server.
    /// </summary>
    public sealed class ChunkServiceClient : IDisposable
    {
        private readonly YetAnotherHttpHandler _handler;
        private readonly HttpClient _httpClient;
        private readonly GrpcChannel _channel;
        private readonly TerraProc.Grpc.Terrain.TerrainClient _client;

        /// <summary>
        /// Creates a new instance of the ChunkServiceClient.
        /// </summary>
        /// <param name="address">Address of the gRPC server.</param>
        public ChunkServiceClient(string address)
        {
            _handler = new YetAnotherHttpHandler { Http2Only = true };
            _httpClient = new HttpClient(_handler);
            _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions { HttpHandler = _handler });
            _client = new TerraProc.Grpc.Terrain.TerrainClient(_channel);
        }

        /// <summary>
        /// Get a chunk from the server asynchronously.
        /// </summary>
        /// <param name="x">X coordinate of the chunk.</param>
        /// <param name="y">Y coordinate of the chunk.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Requested chunk.</returns>
        public Task<TerraProc.Grpc.Chunk> GetChunkAsync(int x, int y, CancellationToken ct = default)
        {
            var coords = new TerraProc.Grpc.ChunkCoords { X = x, Y = y };
            return _client.GetChunkAsync(coords, cancellationToken: ct, deadline: DateTime.UtcNow.AddSeconds(5)).ResponseAsync;
        }

        /// <summary>
        /// Dispose the gRPC channel.
        /// </summary>
        public void Dispose()
        {
            _handler?.Dispose();
            _httpClient?.Dispose();
            _channel?.Dispose();
        }
    }
}