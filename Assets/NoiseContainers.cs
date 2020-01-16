using System;
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

        public abstract NoiseContainer Copy();

        public NoiseContainer SubShape(int[] shape) {
            if (this.shape.Equals(shape)) return this.Copy();
            int dim = shape.Length;
            Assert.AreEqual(dim, this.shape.Length);
            for (int i = 0; i < dim; i++) Assert.IsTrue(this.shape[i] >= shape[i]);

            NoiseVolume noiseVolume = new NoiseVolume(shape);
            NDimEnumerator nDimEnumerator = new NDimEnumerator(shape);
            float value;
            while (nDimEnumerator.MoveNext()) {
                value = this.Get(nDimEnumerator.Current);
                noiseVolume.Set(value);
            }
            return noiseVolume;
        }

        public abstract void Bake();
    }

    class NoiseTextureGray : NoiseContainer {
        private readonly Texture2D texture;
        
        public NoiseTextureGray(int width, int height) : base(new int[] { width, height }) {
            this.texture = new Texture2D(width, height);
        }

        public NoiseTextureGray(Texture2D texture) : base(new int[] { texture.width, texture.height}) {
            this.texture = texture;
        }

        public override void Bake() {
            this.texture.filterMode = FilterMode.Point;
            this.texture.Apply();
        }

        public Texture2D GetTexture() {
            return this.texture;
        }

        public override NoiseContainer Copy() {
            Texture2D textureCopy = new Texture2D(this.texture.width, this.texture.height);
            Graphics.CopyTexture(this.texture, textureCopy);
            return new NoiseTextureGray(textureCopy);
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

        public NoiseVolume(NDimArray array) : base(array.shape) {
            this.array = array;
        }

        public override void Bake() { }

        public override NoiseContainer Copy() {
            float[] arraySource = this.array.GetArray();
            int length = arraySource.Length;
            float[] arrayTarget = new float[length];
            Array.Copy(arraySource, arrayTarget, length);

            NDimArray array = new NDimArray(this.shape, arrayTarget);
            NoiseVolume noiseVolume = new NoiseVolume(array);
            return noiseVolume;
        }

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
