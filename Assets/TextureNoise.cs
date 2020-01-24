using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System.Linq;

public class TextureNoise : MonoBehaviour {
    public float randomness = .1f;
    public int granularity = 2;

    public int offsetXA = 0;
    public int offsetYA = 0;
    public int offsetXB = 0;
    public int offsetYB = 0;
    public int offsetXC = 0;
    public int offsetYC = 0;

    private readonly int size = 128;

    private NoiseContainer NoiseVolumeA;
    private NoiseContainer NoiseVolumeB;
    private NoiseContainer NoiseVolumeC;

    private MyNoise myNoiseA;
    private MyNoise myNoiseB;
    private MyNoise myNoiseC;

    private new Renderer renderer;

    private int layer;


    void Awake() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1, };

        this.NoiseVolumeA = new NoiseVolume(new int[] { this.size, this.size });
        this.NoiseVolumeB = new NoiseVolume(new int[] { this.size, this.size });
        this.NoiseVolumeC = new NoiseVolume(new int[] { this.size, this.size });

        this.myNoiseA = new MyNoise(this.NoiseVolumeA, this.size, wrappedDimensions);
        this.myNoiseB = new MyNoise(this.NoiseVolumeB, this.size, wrappedDimensions);
        this.myNoiseC = new MyNoise(this.NoiseVolumeC, this.size, wrappedDimensions);

        this.renderer = GetComponent<Renderer>();

        this.layer = 0;
    }

    Texture2D Merge(Texture2D[] textures, float[] weights) {
        int width = -1;
        int height = -1;
        foreach (Texture2D eachTexture in textures) {
            if (width < 0 || width < eachTexture.width) width = eachTexture.width;
            if (height < 0 || height < eachTexture.height) height = eachTexture.height;
        }

        int noTextures = textures.Length;
        Assert.AreEqual(noTextures, weights.Length);
        float sumWeights = weights.Sum();
        for (int i = 0; i < noTextures; i++) weights[i] /= sumWeights;

        Texture2D texture = new Texture2D(width, height), everyTexture;
        Color eachColor, newColor;
        float r, g, b;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                r = 0f;
                g = 0f;
                b = 0f;
                for (int i = 0; i < noTextures; i++) {
                    everyTexture = textures[i];
                    eachColor = everyTexture.GetPixel(x, y);
                    r += eachColor.r * weights[i];
                    g += eachColor.g * weights[i];
                    b += eachColor.b * weights[i];
                }
                newColor = new Color(r, g, b);
                texture.SetPixel(x, y, newColor);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }

    void Start() {
    }

    void Update() {
        UnityEngine.Random.InitState(3434463);

        this.myNoiseA.Generate(this.randomness * .25f, this.granularity * 4);
        this.myNoiseB.Generate(this.randomness * .5f, this.granularity * 2);
        this.myNoiseC.Generate(this.randomness * 1f, this.granularity * 1);

        Texture2D textureA = NoiseContainer.GetTexture(this.NoiseVolumeA, this.offsetXA, this.offsetYA);
        Texture2D textureB = NoiseContainer.GetTexture(this.NoiseVolumeB, this.offsetXB, this.offsetYB);
        Texture2D textureC = NoiseContainer.GetTexture(this.NoiseVolumeC, this.offsetXC, this.offsetYC);

        float[] weights = new float[] { 1f, .5f, .25f };

        Texture2D merged = Merge(new Texture2D[] { textureA, textureB, textureC }, weights);

        // this.renderer.material.mainTexture = merged;
        this.renderer.sharedMaterial.mainTexture = merged;

        layer = (layer + 1) % this.size;
    }

    // https://www.youtube.com/watch?v=WP-Bm65Q-1Y&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=2
}
