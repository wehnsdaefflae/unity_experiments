using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System.Linq;

public class UpdateTexture : MonoBehaviour {
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

    private NoiseGeneration myNoiseA;
    private NoiseGeneration myNoiseB;
    private NoiseGeneration myNoiseC;

    private new Renderer renderer;

    private int layer;


    void Awake() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1, };

        this.NoiseVolumeA = new NoiseVolume(new int[] { this.size, this.size });
        this.NoiseVolumeB = new NoiseVolume(new int[] { this.size, this.size });
        this.NoiseVolumeC = new NoiseVolume(new int[] { this.size, this.size });

        this.myNoiseA = new NoiseGeneration(this.NoiseVolumeA, this.size, wrappedDimensions);
        this.myNoiseB = new NoiseGeneration(this.NoiseVolumeB, this.size, wrappedDimensions);
        this.myNoiseC = new NoiseGeneration(this.NoiseVolumeC, this.size, wrappedDimensions);

        this.renderer = GetComponent<Renderer>();

        this.layer = 0;
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
