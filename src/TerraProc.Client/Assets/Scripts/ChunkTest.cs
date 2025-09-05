using System;
using System.Runtime.InteropServices;
using System.Threading;
using GrpcClient;
using Renderer;
using UnityEngine;

namespace DefaultNamespace
{
    public class ChunkTest : MonoBehaviour
    {
        [SerializeField] private string serverUrl = "http://localhost:5000";
        [SerializeField] private Vector2[] chunkCoords;
        [SerializeField] private Material chunkLitMaterial;
        private ChunkServiceClient _client;

        private void Awake()
        {
            _client = new ChunkServiceClient(serverUrl);
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }

        void Start()
        {
            foreach (var vec in chunkCoords)
                RenderSingle((int)vec.x, (int)vec.y);
        }

        private async void RenderSingle(int x, int y)
        {
            Debug.Log($"Fetching chunk at ({x}, {y}) from {serverUrl}");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                var chunk = await _client.GetChunkAsync(x, y, cts.Token);
                Debug.Log($"Received chunk at ({x}, {y}) with size: {chunk.CalculateSize()}");

                var heights = MemoryMarshal.Cast<byte, ushort>(chunk.Heights.Span).ToArray(); // Should be LE
                var materials = chunk.Materials.ToByteArray();
                const int size = 64; // TODO: transfer in the chunk message

                var go = new GameObject($"Chunk({x},{y})");
                var mf = go.AddComponent<MeshFilter>();
                var mr = go.AddComponent<MeshRenderer>();
                mf.sharedMesh = ChunkRenderer.Render(heights, materials, size, .01f);
                mr.sharedMaterial = chunkLitMaterial;
            }
            catch (Grpc.Core.RpcException e)
            {
                Debug.LogError($"gRPC Error: {e.Status.Detail}");
            }
            catch (OperationCanceledException e)
            {
                Debug.LogError($"Request timed out: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error: {e.Message}");
            }
        }
    }
}