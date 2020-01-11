using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System.Linq;

public class TextureNoise : MonoBehaviour {
    public int size = 128;
    public float randomness = .1f;
    public float[] noiseArray;
    private NoiseVolume NoiseVolume;
    public int layer;

    private static Texture2D CubeToTexture(NoiseVolume noiseVolume, int layer) {
        int[] shape = noiseVolume.shape;
        Assert.AreEqual(noiseVolume.dimensionality, 3);
        Texture2D texture = new Texture2D(shape[0], shape[1]);

        float value;
        Color color;
        for (int x = 0; x < shape[0]; x++) {
            for (int y = 0; y < shape[1]; y++) {
                value = noiseVolume.Get(x, y, layer);
                color = new Color(value, value, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }

    void Start() {
        NoiseVolume = new NoiseVolume(new int[] { this.size, this.size });
        MyNoise myNoise = new MyNoise(NoiseVolume, this.size, new int[] { 0, 1 });
        myNoise.Generate(this.randomness);
    }

    void Update() {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = TextureNoise.CubeToTexture(this.NoiseVolume, this.layer);
        // renderer.material.mainTexture = Generate();
    }

    Texture2D Generate() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1 };
        // int[] wrappedDimensions = new int[] { };

        int width = size + (wrappedDimensions.Contains(0) ? 0 : 1);
        int height = size + (wrappedDimensions.Contains(1) ? 0 : 1);
        NoiseTextureGray noiseContainer = new NoiseTextureGray(width, height);

        MyNoise myNoise = new MyNoise(noiseContainer, size, wrappedDimensions);
        myNoise.Generate(this.randomness);
        this.noiseArray = noiseContainer.GetArray();
        return noiseContainer.texture;
    }

}