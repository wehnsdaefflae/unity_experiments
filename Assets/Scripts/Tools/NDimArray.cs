using UnityEngine;
using UnityEngine.Assertions;

namespace Assets {
    class NDimArray {
        private readonly float[] array;
        public readonly int[] shape;

        public NDimArray(int[] shape, float[] initialArray) {
            this.shape = shape;
            int length = 1;
            foreach (int d in shape) length *= d;
            Assert.AreEqual(length, initialArray.Length);
            array = initialArray;
        }

        public NDimArray(int[] shape) {
            this.shape = shape;
            int length = 1;
            foreach (int d in shape) length *= d;
            array = new float[length];
        }

        public float[] GetArray() {
            return this.array;
        }

        public override string ToString() {
            return this.array.ToString();
        }

        private int Linearize(int[] coordinates) {
            int noCoordinates = coordinates.Length;
            Assert.AreEqual(noCoordinates, this.shape.Length);
            int index = 0;
            int c, d;
            int j = 1;
            for (int i = 0; i < noCoordinates; i++) {
                c = coordinates[i];
                d = this.shape[i];
                index += Mathf.RoundToInt((c % d) * j);
                j *= d;
            }

            return index;
        }

        public void Set(float value, params int[] coordinates) {
            int index = this.Linearize(coordinates);
            this.array[index] = value;
        }

        public float Get(params int[] coordinates) {
            int index = this.Linearize(coordinates);
            return this.array[index];
        }
    }
}
