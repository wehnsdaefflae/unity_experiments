using Assets;
using UnityEngine;
using UnityEngine.Assertions;

public static class MeshGenerator {
    public static MeshData GenerateTerrainMesh(NDimArray depthMap) {
        Assert.AreEqual(depthMap.shape.Length, 2);
        int width = depthMap.shape[0];
        int height = depthMap.shape[1];

        float topLeftX = (width - 1f) / -2f;
        float topLeftZ = (height - 1f) / 2f;

        MeshData meshData = new MeshData(width, height);
        int indexVertex = 0;
        Vector3 vector;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                vector = new Vector3(topLeftX + x, depthMap.Get(x, y), topLeftZ - y);
                meshData.vertices[indexVertex] = vector;
                meshData.uvs[indexVertex] = new Vector2(x / (float) width, y / (float) height);

                if (x < width - 1 && y < height - 1) {
                    meshData.addTriangle(indexVertex, indexVertex + width + 1, indexVertex + width);
                    meshData.addTriangle(indexVertex + width + 1, indexVertex, indexVertex + 1);
                }

                indexVertex++;
            }
        }

        return meshData;

    }
}


public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    private int indexTriangle;


    public MeshData(int meshWidth, int meshHeight) {
        this.vertices = new Vector3[meshWidth * meshHeight];
        this.triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        this.uvs = new Vector2[meshWidth * meshHeight];

        this.indexTriangle = 0;
    }

    public void addTriangle(int a, int b, int c) {
        this.triangles[this.indexTriangle++] = a;
        this.triangles[this.indexTriangle++] = b;
        this.triangles[this.indexTriangle++] = c;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = this.vertices;
        mesh.triangles = this.triangles;
        mesh.uv = this.uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}