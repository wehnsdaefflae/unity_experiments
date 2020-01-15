using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System.Linq;

public class TextureNoise : MonoBehaviour {
    public int size = 512;
    public float randomness = .1f;
    private NoiseContainer NoiseVolume;
    private MyNoise myNoise;
    private MyNoiseNew myNoiseNew;


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
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1 };

        int width = size + (wrappedDimensions.Contains(0) ? 0 : 1);
        int height = size + (wrappedDimensions.Contains(1) ? 0 : 1);
        //this.NoiseVolume = new NoiseVolume(new int[] { width, height });
        this.NoiseVolume = new NoiseTextureGray(width, height);

        // this.myNoise = new MyNoise(this.NoiseVolume, this.size, new int[] { 0, 1 });
        this.myNoiseNew = new MyNoiseNew(this.NoiseVolume, this.size, new int[] { 0, 1 });
    }

    void Update() {
        Renderer renderer = GetComponent<Renderer>();

        // this.myNoise.Generate(this.randomness);
        this.myNoiseNew.Generate(this.randomness);

        // renderer.material.mainTexture = TextureNoise.CubeToTexture(this.NoiseVolume, this.layer);
        renderer.material.mainTexture = ((NoiseTextureGray) this.NoiseVolume).texture;
    }

}