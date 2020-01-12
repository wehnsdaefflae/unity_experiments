using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Assets {
    class NDimEnumerator : IEnumerator<int[]> {
        public readonly int dimensions;
        private int[] current;
        private readonly int[] target;

        public int[] Current {
            get {
                int[] returnArray = new int[this.dimensions];
                Array.Copy(current, 0, returnArray, 0, this.dimensions);
                return returnArray;
            }
        }

        object IEnumerator.Current {
            get {
                int[] returnArray = new int[this.dimensions];
                Array.Copy(current, 0, returnArray, 0, this.dimensions);
                return returnArray;
            }
        }

        public void Dispose() {
            this.current[this.dimensions - 1] = -1;
        }

        public void Reset() {
            this.current = new int[this.dimensions];
        }

        public NDimEnumerator(int[] target) {
            this.dimensions = target.Length;
            this.target = target;
            this.current = new int[this.dimensions];
            for (int i = 0; i < this.dimensions; i++) {
                if (i == this.dimensions - 1) Assert.IsTrue(0 < this.target[i]);
                else Assert.IsTrue(this.target[i] >= 0);
            }
        }

        public bool MoveNext() {
            int c;
            for (int i = this.dimensions - 1; i >= 0; i--) {
                c = current[i];
                if (c < 0) return false;

                // below target
                if (c < this.target[i] - 1) {
                    this.current[i] = c + 1;
                    break;

                // at target 
                } else {
                    this.current[i] = 0;
                    if (i == 0) return false;
                }
            }
            return true;
        }
    }
}
