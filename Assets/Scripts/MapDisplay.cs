using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;


    public void DrawTexture(Texture2D texture2D) {
        this.textureRenderer.sharedMaterial.mainTexture = texture2D;
        this.textureRenderer.transform.localScale = new Vector3(texture2D.width, 1, texture2D.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture2D) {
        this.meshFilter.sharedMesh = meshData.CreateMesh();
        this.meshRenderer.sharedMaterial.mainTexture = texture2D;
    }
}
