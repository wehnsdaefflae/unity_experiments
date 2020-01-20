using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System.Linq;

public class TextureNoise : MonoBehaviour {
    public int size = 128;
    public float randomness = .1f;
    private NoiseContainer NoiseVolume;
    private MyNoise myNoise;
    private MyNoiseNew myNoiseNew;


    private static Texture2D GetTexture(NoiseContainer noiseVolume, params int[] layer) {
        int[] shape = noiseVolume.shape;
        int dimensions = layer.Length;
        Assert.AreEqual(dimensions + 2, shape.Length);

        Texture2D texture = new Texture2D(shape[0], shape[1]);

        int[] coordinates = new int[shape.Length];
        for (int i = 2; i < coordinates.Length; i++) coordinates[i] = layer[i];
        float value;
        Color color;
        for (int x = 0; x < shape[0]; x++) {
            coordinates[0] = x;
            for (int y = 0; y < shape[1]; y++) {
                coordinates[1] = y;
                value = noiseVolume.Get(coordinates);
                color = new Color(value, value, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }
    

    void Start() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1 };
        // int[] wrappedDimensions = new int[] {};

        int width = size + (wrappedDimensions.Contains(0) ? 0 : 1);
        int height = size + (wrappedDimensions.Contains(1) ? 0 : 1);
        
        this.NoiseVolume = new NoiseVolume(new int[] { width, height, this.size });
        // this.NoiseVolume = new NoiseTextureGray(width, height);

        // this.myNoise = new MyNoise(this.NoiseVolume, this.size, wrappedDimensions);
        this.myNoiseNew = new MyNoiseNew(this.NoiseVolume, this.size, wrappedDimensions);
    }

    void Update() {
        Renderer renderer = GetComponent<Renderer>();

        // this.myNoise.Generate(this.randomness);
        if (this.myNoiseNew != null) this.myNoiseNew.Generate(this.randomness);

        // renderer.material.mainTexture = TextureNoise.CubeToTexture((NoiseVolume) this.NoiseVolume, 0);
        // renderer.material.mainTexture = ((NoiseTextureGray) this.NoiseVolume).GetTexture();

        if (this.NoiseVolume != null) renderer.material.mainTexture = TextureNoise.GetTexture(this.NoiseVolume, 0);
    }

}
 