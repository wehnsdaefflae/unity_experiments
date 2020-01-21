using UnityEngine;
using Assets;
using UnityEngine.Assertions;


public class TextureNoise : MonoBehaviour {
    public int size = 256;
    public float randomness = .1f;

    private NoiseContainer NoiseVolume;
    private MyNoise myNoise;
    private MyNoiseNew myNoiseNew;

    private int layer;


    void Awake() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1, 2 };

        this.NoiseVolume = new NoiseVolume(new int[] { this.size, this.size, this.size });

        this.myNoiseNew = new MyNoiseNew(this.NoiseVolume, this.size, wrappedDimensions);

        this.myNoiseNew.Generate(this.randomness);

        this.layer = 0;
    }

    void Start() {
    }

    void Update() {
        Renderer renderer = GetComponent<Renderer>();

        renderer.material.mainTexture = NoiseContainer.GetTexture(this.NoiseVolume, this.layer);

        layer = (layer + 1) % this.size;
    }

}
 