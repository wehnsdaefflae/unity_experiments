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


    /*
    public override Texture2D GetTexture(int layer, NoiseVolume noiseVolume) {
        int[] shape = this.NoiseVolume.shape;
        

        Texture2D texture = new Texture2D(shape[0], shape[1]);

        float value;
        Color color;
        for (int x = 0; x < shape[0]; x++) {
            for (int y = 0; y < shape[1]; y++) {
                value = this.Get(x, y, layer);
                color = new Color(value, value, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }
    */
    

    void Start() {
        Assert.IsTrue((this.size & (this.size - 1)) == 0);

        int[] wrappedDimensions = new int[] { 0, 1 };
        // int[] wrappedDimensions = new int[] {};

        int width = size + (wrappedDimensions.Contains(0) ? 0 : 1);
        int height = size + (wrappedDimensions.Contains(1) ? 0 : 1);
        
        // this.NoiseVolume = new NoiseVolume(new int[] { width, height, 1 });
        this.NoiseVolume = new NoiseTextureGray(width, height);

        // this.myNoise = new MyNoise(this.NoiseVolume, this.size, wrappedDimensions);
        this.myNoiseNew = new MyNoiseNew(this.NoiseVolume, this.size, wrappedDimensions);
    }

    void Update() {
        Renderer renderer = GetComponent<Renderer>();

        // this.myNoise.Generate(this.randomness);
        this.myNoiseNew.Generate(this.randomness);

        // renderer.material.mainTexture = TextureNoise.CubeToTexture((NoiseVolume) this.NoiseVolume, 0);
        renderer.material.mainTexture = ((NoiseTextureGray) this.NoiseVolume).GetTexture();
    }

}
 