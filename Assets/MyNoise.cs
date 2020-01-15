using System;
using System.Linq;
using System.Numerics;
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


        private void Scaffold() {
            int[] states = new int[this.container.dimensionality];
            for (int i = 0; i < states.Length; i++) states[i] = 2;
            NDimEnumerator nDimEnumerator = new NDimEnumerator(states);

            int[] coordinates;
            while (nDimEnumerator.MoveNext()) {
                coordinates = nDimEnumerator.Current;
                for (int i = 0; i < coordinates.Length; i++) coordinates[i] *= this.size;
                this.container.Set(UnityEngine.Random.value, coordinates);
            }

        }


        public void Generate(float randomness) {
            this.Scaffold();

            int sizeCube = this.size;
            // sizeCube must be power of 2

            int noTiles;
            NDimEnumerator generatorCoordinates;
            int[] coordinates;
            int[] tile_c = new int[this.container.dimensionality];

            while (sizeCube >= 2) {
                noTiles = this.size / sizeCube;
                for (int i = 0; i < tile_c.Length; i++) tile_c[i] = noTiles;
                generatorCoordinates = new NDimEnumerator(tile_c);

                while (generatorCoordinates.MoveNext()) {  // do this in parallel
                    coordinates = generatorCoordinates.Current;
                    for (int i = 0; i < coordinates.Length; i++) coordinates[i] *= sizeCube;
                    this.Interpolate(coordinates, sizeCube, randomness);
                }
                sizeCube /= 2;
            }

            this.container.Bake();
            return;

        }

        private static int Power(int b, int e) {
            int r = 1;
            for (int i = 0; i < e; i++) r *= b;      
            return r;
        }


        public static int[][][] GetBorders(int[] pointA, int[] pointB) {
            int dimension = pointA.Length;
            Assert.AreEqual(dimension, pointB.Length);

            // initialize border structure (2*d-array of borders (2-array of points (d-array of integers)))
            int[][][] borders = new int[dimension * 2][][];
            int[][] eachBorder;
            for (int i = 0; i < borders.Length; i++) {
                eachBorder = new int[2][];
                borders[i] = eachBorder;
                for (int j = 0; j < eachBorder.Length; j++) eachBorder[j] = new int[dimension];
            }

            // declare loop variables
            int[] pointBorder;
            int[][] borderLo, borderHi;

            // for each dimension
            for (int i = 0; i < dimension; i++) {
                // get low border
                borderLo = borders[i];

                // point a is original point a, point b is original point b EXCEPT along one dimension
                Array.Copy(pointA, borderLo[0], dimension);
                pointBorder = borderLo[1];
                for (int j = 0; j < dimension; j++) {
                    pointBorder[j] = i == j ? pointA[j] : pointB[j];
                }

                // get high border
                borderHi = borders[i + dimension];

                // point a is original point a EXCEPT along one dimension, point b is original point b
                Array.Copy(pointB, borderHi[1], dimension);
                pointBorder = borderHi[0];
                for (int j = 0; j < dimension; j++) {
                    pointBorder[j] = i == j ? pointB[j] : pointA[j];
                }
            }

            return borders;
        }

        private void Interpolate(int[] origin, int sizeWindow, float randomness) {
            NDimPermutator nDimPermutator;
            bool[] coordinateComposition;
            int[] pointEnd, pointMid;
            bool indexSource;
            int midWay = sizeWindow / 2;

            int dim = this.container.dimensionality;
            Assert.AreEqual(dim, origin.Length);

            for (int i = 0; i < dim; i++) {
                nDimPermutator = new NDimPermutator(dim, i);
                while (nDimPermutator.MoveNext()) {
                    coordinateComposition = nDimPermutator.Current;
                    pointEnd = new int[dim];
                    pointMid = new int[dim];
                    for (int j = 0; j < dim; j++) {
                        indexSource = coordinateComposition[j];
                        pointEnd[j] = indexSource ? 0 : sizeWindow;
                        pointMid[j] = indexSource ? 0 : midWay;
                    }
                    
                }

            }


            int[] midPoint = new int[origin.Length];
            Array.Copy(origin, 0, midPoint, 0, origin.Length);
            for (int i = 0; i < midPoint.Length; i++) midPoint[i] += sizeWindow / 2;

            int[] borders = new int[this.container.dimensionality];
            for (int i = 0; i < borders.Length; i++) borders[i] = 2;
            NDimEnumerator nDimEnumerator = new NDimEnumerator(borders);

            int[] t_c;
            while (nDimEnumerator.MoveNext()) {     // for each vertex
                t_c = nDimEnumerator.Current;
                for (int i = 0; i < t_c.Length; i++) t_c[i] *= this.size;

            }

                // for each vertex
                //  combine all coordinates with midPoint
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
            float r = UnityEngine.Random.Range(-randomness, randomness);
            return Mathf.Max(0f, Mathf.Min(1f, value + r));
        }

        private static float Randomize(float value, float randomness) {
            float minDistance = Mathf.Min(value, 1f - value);
            float r = UnityEngine.Random.Range(0f, minDistance);
            if (value < UnityEngine.Random.Range(0f, 1f))
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
            this.container.Set(UnityEngine.Random.value, 0, 0);
            this.container.Set(UnityEngine.Random.value, 0, this.size);
            this.container.Set(UnityEngine.Random.value, this.size, 0);
            this.container.Set(UnityEngine.Random.value, this.size, this.size);
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
