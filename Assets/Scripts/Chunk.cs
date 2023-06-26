using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] UV;
    public int width = 10;
    public int height = 10;
    public float scale = 1f;
    public float maxHeight = 1f;
    MeshCollider meshCollider;

    public Chunk(float x, float y) { }

    private void CreateShape()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];
        UV = new Vector2[vertices.Length]; // Initialize the UV array
        int vertexIndex = 0;

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                float heightValue = Mathf.PerlinNoise(x * scale, y * scale) * maxHeight;
                Debug.Log(heightValue);
                vertices[vertexIndex] = new Vector3(x, heightValue, y);
                UV[vertexIndex] = new Vector2((float)x / width, (float)y / height); // Calculate UV coordinates
                vertexIndex++;
            }
        }

        triangles = new int[width * height * 6];
        int triangleIndex = 0;
        int vertexCount = width + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int topLeft = y * vertexCount + x;
                int topRight = topLeft + 1;
                int bottomLeft = (y + 1) * vertexCount + x;
                int bottomRight = bottomLeft + 1;

                triangles[triangleIndex] = topLeft;
                triangles[triangleIndex + 1] = bottomLeft;
                triangles[triangleIndex + 2] = topRight;
                triangles[triangleIndex + 3] = topRight;
                triangles[triangleIndex + 4] = bottomLeft;
                triangles[triangleIndex + 5] = bottomRight;

                triangleIndex += 6;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UV;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }
}
