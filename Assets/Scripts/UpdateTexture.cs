using UnityEngine;
using Assets;
using UnityEngine.Assertions;

public class UpdateTexture : MonoBehaviour {
    public float randomness = .1f;
    public int granularity = 2;

    public int offsetX = 0;
    public int offsetY = 0;

    public bool autoUpdate = true;

    private readonly int size = 128;
    private NoiseContainer NoiseVolume;
    private new Renderer renderer;


    private static Texture2D ArrayToTexture(NDimArray nDimArray, int offsetX, int offsetY) {
        Assert.IsTrue(nDimArray.shape.Length == 2);
        int width = nDimArray.shape[0];
        int height = nDimArray.shape[1];
        Texture2D texture2D = new Texture2D(width, height);

        float value;
        Color color;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                value = nDimArray.Get(x, y);
                color = new Color(value, value, value);
                texture2D.SetPixel(MathTools.Modulo(x + offsetX, width), MathTools.Modulo(y + offsetY, height), color);
            }
        }

        texture2D.filterMode = FilterMode.Point;
        texture2D.Apply();

        return texture2D;
    }


    private void Awake() {
    }

    private void Start() {
    }

    private void Update() {
    }

    public void MyAwake() {
        int[] wrappedDimensions = new int[] { 0, 1, };

        this.NoiseVolume = new NoiseVolume(this.size, 2, wrappedDimensions);
        this.renderer = GetComponent<Renderer>();
    }


    private NDimArray Compose(int octaves, float persistance, float lacunarity) {
        float frequency = 1f;   // randomness
        float amplitude = 1f;   // minValue, maxValue


        for (int i = 0; i < octaves - 1; i++) {

        }
        NoiseVolume noiseContainer = new NoiseVolume(this.size, 2, new int[] { });

        return noiseContainer.GetSlice(new int[] { });

    }

    public void MyUpdate() {
        UnityEngine.Random.InitState(3434463);

        NoiseGeneration.Generate(this.NoiseVolume, this.randomness * 1f, this.granularity * 1, 0f, 1f);
        NDimArray map = this.NoiseVolume.GetSlice(new int[] { });
        Texture2D texture = UpdateTexture.ArrayToTexture(map, this.offsetX, this.offsetY);
        this.renderer.sharedMaterial.mainTexture = texture;
    }

    // https://www.youtube.com/watch?v=WP-Bm65Q-1Y&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=2
}
