﻿using System.Collections;
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
    private NDimPermutator NDimPermutator;
    public bool[] permutation;
    public bool permutationBool;


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

        this.NDimPermutator = new NDimPermutator(3, 3);

        int[] wrappedDimensions = new int[] { 0, 1 };

        int width = size + (wrappedDimensions.Contains(0) ? 0 : 1);
        int height = size + (wrappedDimensions.Contains(1) ? 0 : 1);
        //this.NoiseVolume = new NoiseVolume(new int[] { width, height });
        this.NoiseVolume = new NoiseTextureGray(width, height);

        this.myNoise = new MyNoise(NoiseVolume, this.size, new int[] { 0, 1 });
    }

    void Update() {
        this.permutationBool = this.NDimPermutator.MoveNext();
        this.permutation = this.NDimPermutator.Current;
        int[][][] borders = MyNoiseNew.GetBorders(new int[] { 0, 0, 0 }, new int[] { 1, 1, 1 });

        Renderer renderer = GetComponent<Renderer>();

        this.myNoise.Generate(this.randomness);

        // renderer.material.mainTexture = TextureNoise.CubeToTexture(this.NoiseVolume, this.layer);
        renderer.material.mainTexture = ((NoiseTextureGray) this.NoiseVolume).texture;
    }

}