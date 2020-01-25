using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


namespace Assets {
    public class TextureTools
    {
        public static Texture2D Merge(Texture2D[] textures, float[] weights)
        {
            int width = -1;
            int height = -1;
            foreach (Texture2D eachTexture in textures)
            {
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

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    r = 0f;
                    g = 0f;
                    b = 0f;
                    for (int i = 0; i < noTextures; i++)
                    {
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
    }
}