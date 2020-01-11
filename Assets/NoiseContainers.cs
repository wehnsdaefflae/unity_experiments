using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets {
    abstract class NoiseContainer {
        public readonly int[] shape;
        public readonly int dimensionality;

        public abstract float[] GetArray();

        protected NoiseContainer(int[] shape) {
            this.shape = shape;
            this.dimensionality = shape.Length;
        }

        private int[] Wrap(int[] coordinates) {
            Assert.AreEqual(coordinates.Length, this.dimensionality);

            int[] wrappedCoordinates = new int[this.dimensionality];
            for (int i = 0; i < this.dimensionality; i++) {
                wrappedCoordinates[i] = coordinates[i] % this.shape[i];
            }
            return wrappedCoordinates;
        }

        protected abstract void _Set(float value, params int[] coordinates);

        public void Set(float value, params int[] coordinates) {
            int[] wrapped = this.Wrap(coordinates);
            this._Set(value, wrapped);
        }

        protected abstract float _Get(params int[] coordinates);

        public float Get(params int[] coordinates) {
            int[] wrapped = this.Wrap(coordinates);
            return this._Get(wrapped);
        }

        public abstract void Bake();
    }

    class NoiseTextureGray : NoiseContainer {
        public readonly Texture2D texture;
        
        public NoiseTextureGray(int width, int height) : base(new int[] { width, height }) {
            this.texture = new Texture2D(width, height);
        }

        public override void Bake() {
            this.texture.filterMode = FilterMode.Point;
            this.texture.Apply();
        }

        public override float[] GetArray() {
            float[] array = new float[texture.width * texture.height];
            int i = 0;
            for (int x = 0; x < this.texture.width; x++) {
                for (int y = 0; y < texture.height; y++) {
                    array[i] = this._Get(x, y);
                    i++;
                }
            }
            return array;
        }

        protected override float _Get(params int[] coordinates) {
            Color color = this.texture.GetPixel(coordinates[0], coordinates[1]);
            return color.r;
        }

        protected override void _Set(float value, params int[] coordinates) {
            Color color = new Color(value, value, value);
            this.texture.SetPixel(coordinates[0], coordinates[1], color);
        }

    }

    class NoiseVolume : NoiseContainer {
        private readonly NDimArray array;

        public NoiseVolume(int[] shape) : base(shape) {
            this.array = new NDimArray(shape);
        }

        public override void Bake() { }

        public override float[] GetArray() {
            return this.array.GetArray();
        }

        protected override float _Get(params int[] coordinates) {
            return this.array.Get(coordinates);
        }

        protected override void _Set(float value, params int[] coordinates) {
            this.array.Set(value, coordinates);
        }
    }
}
