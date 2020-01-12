using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets {

    class MyNoiseNew {
        public readonly int size;
        private readonly NoiseContainer container;

        public MyNoiseNew(NoiseContainer container, int size, int[] wrappedDimensions) {
            Assert.IsTrue(0 < size);
            // is size power of 2?
            Assert.IsTrue((size & (size - 1)) == 0);

            int eachDimension;
            for (int i = 0; i < container.dimensionality; i++) {
                eachDimension = container.shape[i];
                if (wrappedDimensions.Contains(i)) {
                    Assert.IsTrue(container.shape[i] >= size);
                    Assert.IsTrue((eachDimension & (eachDimension - 1)) == 0);
                } else {
                    Assert.IsTrue(container.shape[i] - 1 >= size);
                    Assert.IsTrue(((eachDimension - 1) & (eachDimension - 2)) == 0);
                }
            }

            this.size = size;
            this.container = container;
        }

        public void Generate(float randomness) {
            /*
            // scaffold
            NDimEnumerator nDimEnumerator = new NDimEnumerator(new int[] { 2, 2, 2 });
            int[] coordinates = new int[this.container.dimensionality];
            do {
                for (int i = 0; i < this.container.dimensionality; i++)
                    coordinates[i] = nDimEnumerator.Current[i] * this.size;

                this.container.Set(Random.Range(0f, 1f), coordinates);
            } while (nDimEnumerator.MoveNext());


            // filling

            int sizeCube = this.size;  // sizeCube must be power of 2

            int limit;
            while (sizeCube >= 2) {
                limit = this.size / sizeCube + 1;
                nDimEnumerator = new NDimEnumerator(new int[] { limit, limit, limit });

                // todo: each sizeCube in parallel
                do {
                    for (int i = 0; i < this.container.dimensionality; i++)
                        coordinates[i] = nDimEnumerator.Current[i] * sizeCube;

                    this.Interpolate(sizeCube, randomness, coordinates);
                } while (nDimEnumerator.MoveNext());

                sizeCube /= 2;
            }

            this.container.Bake();
            */
            return;
        }

        private void Interpolate(int sizeWindow, float randomness, params int[] coordinates) {

        }
    }

        class MyNoise {
        public readonly int size;
        private readonly NoiseContainer container;

        public MyNoise(NoiseContainer container, int size, int[] wrappedDimensions) {
            // for now
            Assert.AreEqual(container.dimensionality, 2);

            Assert.IsTrue(0 < size);
            // is size power of 2?
            Assert.IsTrue((size & (size - 1)) == 0);

            int eachDimension;
            for (int i = 0; i < container.dimensionality; i++) {
                eachDimension = container.shape[i];
                if (wrappedDimensions.Contains(i)) {
                    Assert.IsTrue(container.shape[i] >= size);
                    Assert.IsTrue((eachDimension & (eachDimension - 1)) == 0);
                } else {
                    Assert.IsTrue(container.shape[i] - 1 >= size);
                    Assert.IsTrue(((eachDimension - 1) & (eachDimension - 2)) == 0);
                }
            }

            this.size = size;
            this.container = container;
        }

        private static float RandomizeOld(float value, float randomness) {
            float r = Random.Range(-randomness, randomness);
            return Mathf.Max(0f, Mathf.Min(1f, value + r));
        }

        private static float Randomize(float value, float randomness) {
            float minDistance = Mathf.Min(value, 1f - value);
            float r = Random.Range(0f, minDistance);
            if (value < Random.Range(0f, 1f))
                return value + r;
            return value - r;

        }

        private void Interpolate(int XtopLeft, int YtopLeft, int sizeWindow, float randomness) {
            // sizeWindow must be power of 2
            float topLeft = this.container.Get(XtopLeft, YtopLeft);
            float topRight = this.container.Get(XtopLeft, YtopLeft + sizeWindow);
            float botLeft = this.container.Get(XtopLeft + sizeWindow, YtopLeft);
            float botRight = this.container.Get(XtopLeft + sizeWindow, YtopLeft + sizeWindow);

            float topCenter = MyNoise.Randomize((topLeft + topRight) / 2f, randomness);
            float leftCenter = MyNoise.Randomize((topLeft + botLeft) / 2f, randomness);
            float rightCenter = MyNoise.Randomize((topRight + botRight) / 2f, randomness);
            float botCenter = MyNoise.Randomize((botLeft + botRight) / 2f, randomness);
            float center = MyNoise.Randomize((topLeft + topRight + botLeft + botRight) / 4f, randomness);

            this.container.Set(topCenter, XtopLeft + sizeWindow / 2, YtopLeft);
            this.container.Set(leftCenter, XtopLeft, YtopLeft + sizeWindow / 2);
            this.container.Set(rightCenter, XtopLeft + sizeWindow, YtopLeft + sizeWindow / 2);
            this.container.Set(botCenter, XtopLeft + sizeWindow / 2, YtopLeft + sizeWindow);
            this.container.Set(center, XtopLeft + sizeWindow / 2, YtopLeft + sizeWindow / 2);
        }
        
        private void Scaffold() {
            this.container.Set(Random.value, 0, 0);
            this.container.Set(Random.value, 0, this.size);
            this.container.Set(Random.value, this.size, 0);
            this.container.Set(Random.value, this.size, this.size);
        }

        public void Generate(float randomness) {
            this.Scaffold();

            /*
            public void GenerateShader(float randomness) {
                ComputeShader shader;
                int kernelHandle = shader.FindKernel("CSMain");
                RenderTexture texture = new RenderTexture(256, 256, 24);
            }
            */

            int sizeCube = this.size;
            // sizeCube must be power of 2

            int noTiles;
            NDimEnumerator generatorCoordinates;
            int[] coordinates;

            while (sizeCube >= 2) {
                noTiles = this.size / sizeCube;
                generatorCoordinates = new NDimEnumerator(new int[] { noTiles, noTiles });
                while (generatorCoordinates.MoveNext()) {  // do this in parallel
                    coordinates = generatorCoordinates.Current;
                    this.Interpolate(coordinates[0] * sizeCube, coordinates[1] * sizeCube, sizeCube, randomness);
                }
                sizeCube /= 2;
            }

            this.container.Bake();
            return;
        }

    }

}
