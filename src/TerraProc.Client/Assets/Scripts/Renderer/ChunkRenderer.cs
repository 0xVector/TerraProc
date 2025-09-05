using System;
using TerraProc.Grpc;
using UnityEngine;

namespace Renderer
{
    public sealed class ChunkRenderer
    {
        public static Mesh Render(ushort[] heights, byte[] materials, int size, float heightScale)
        {
            var n = heights.Length; //size * size;

            var vertices = new Vector3[n]; // Geometry
            var uvs = new Vector2[n]; // Texture coordinates (not needed for now)
            var triangles = new int[(size - 1) * (size - 1) * 6]; // Triangle faces
            var colors = new Color32[n]; // Vertex colors based on material

            for (var i = 0; i < n; i++)
            {
                var z = heights[i] * heightScale;
                var y = Math.DivRem(i, size, out var x);

                vertices[i] = new Vector3(x, z, y); // Unity uses y as height -> use calculated z
                uvs[i] = new Vector2((float)x / (size - 1), (float)y / (size - 1));
                colors[i] = GetColor(materials[i]);
            }

            int Linearize(int x, int y) => x + y * size;

            var j = 0;
            for (var x = 0; x < (size - 1); x++)
            for (var y = 0; y < (size - 1); y++)
            {
                triangles[j++] = Linearize(x, y);
                triangles[j++] = Linearize(x + 1, y + 1);
                triangles[j++] = Linearize(x + 1, y);

                triangles[j++] = Linearize(x, y);
                triangles[j++] = Linearize(x, y + 1);
                triangles[j++] = Linearize(x + 1, y + 1);
            }


            var mesh = new Mesh(); // {indexFormat = UnityEngine.Rendering.IndexFormat.UInt16};
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.SetColors(colors);

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }


        private static Color32 GetColor(byte material)
        {
            return material switch
            {
                0 => new Color32(0, 0, 0, 255),
                1 => new Color32(50, 50, 50, 255),
                _ => default
            };
        }
    }
}