using UnityEngine;
using Assets;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

public class UpdateTexture : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    public DrawMode drawMode;

    [Range(0f, 1f)]
    public float randomness = .4f;

    public int granularity = 2;

    public int offsetX = 0;
    public int offsetY = 0;

    public bool autoUpdate = true;

    private readonly int size = 128;
    private NoiseContainer NoiseVolume;
    private new Renderer renderer;

    public TerrainType[] regions;


    private static Texture2D ArrayToNoiseMap(NDimArray nDimArray, int offsetX, int offsetY) {
        Assert.IsTrue(nDimArray.shape.Length == 2);
        int width = nDimArray.shape[0];
        int height = nDimArray.shape[1];
        Texture2D texture2D = new Texture2D(width, height);

        float valueMax = Mathf.Max(nDimArray.GetArray());
        float valueMin = Mathf.Min(nDimArray.GetArray());

        float value, valueNormalized; ;
        Color color;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                value = nDimArray.Get(x, y);
                valueNormalized = MathTools.Normalize(value, valueMin, valueMax);
                color = new Color(valueNormalized, valueNormalized, valueNormalized);
                texture2D.SetPixel(MathTools.Modulo(x + offsetX, width), MathTools.Modulo(y + offsetY, height), color);
            }
        }

        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.Apply();

        return texture2D;
    }


    private static Texture2D ArrayToColourMap(NDimArray nDimArray, int offsetX, int offsetY, TerrainType[] regions) {
        Assert.IsTrue(nDimArray.shape.Length == 2);
        int width = nDimArray.shape[0];
        int height = nDimArray.shape[1];
        Texture2D texture2D = new Texture2D(width, height);

        TerrainType[] regionsSorted = new TerrainType[regions.Length];
        Array.Copy(regions, regionsSorted, regionsSorted.Length);
        Array.Sort(regionsSorted, new SorterTerrainTypes());

        float valueMax = Mathf.Max(nDimArray.GetArray());
        float valueMin = Mathf.Min(nDimArray.GetArray());

        float value, valueNormalized;
        Color color;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                value = nDimArray.Get(x, y);
                valueNormalized = MathTools.Normalize(value, valueMin, valueMax);
                foreach (TerrainType region in regionsSorted) {
                    if (region.heightMax < valueNormalized) {
                        continue;
                    }
                    color = region.color;
                    texture2D.SetPixel(MathTools.Modulo(x + offsetX, width), MathTools.Modulo(y + offsetY, height), color);
                    break;
                }
            }
        }

        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
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

        MapDisplay mapDisplay = new MapDisplay();

        if (this.drawMode.Equals(DrawMode.ColourMap)) {
            Texture2D texture2D = UpdateTexture.ArrayToColourMap(map, this.offsetX, this.offsetY, this.regions);
            mapDisplay.DrawTexture(texture2D);
        
        } else if (this.drawMode.Equals(DrawMode.NoiseMap)) {
            Texture2D texture2D = UpdateTexture.ArrayToNoiseMap(map, this.offsetX, this.offsetY);
            mapDisplay.DrawTexture(texture2D);
        
        } else if (this.drawMode.Equals(DrawMode.Mesh)) {
            // GetComponent<MeshFilter>(), GetComponent<MeshRenderer>();
            
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(map);
            Texture2D texture2D = UpdateTexture.ArrayToColourMap(map, this.offsetX, this.offsetY, this.regions);
            mapDisplay.DrawMesh(meshData, texture2D);

        }
    }

    // https://www.youtube.com/watch?v=WP-Bm65Q-1Y&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=2

    private void OnValidate() {
        this.randomness = Mathf.Max(0f, Mathf.Min(1f, this.randomness));
        this.granularity = Mathf.Max(2, Mathf.Min(this.size, this.granularity));
    }
}



[System.Serializable]
public struct TerrainType {
    public string name;
    public Color color;

    [Range(0f, 1f)]
    public float heightMax;
}

public class SorterTerrainTypes : IComparer<TerrainType> {

    public int Compare(TerrainType x, TerrainType y) {
        // Compare y and x in reverse order. 
        return x.heightMax.CompareTo(y.heightMax);
    }
}