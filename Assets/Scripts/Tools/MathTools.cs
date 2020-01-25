using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets {
    public class MathTools {
        public static int Modulo(int a, int b) {
            return ((a % b) + b) % b;
        }

        public static float Randomize(float value, float randomness) {
            return MathTools.Randomize(value, randomness, 0f, 1f);
        }

        public static float Randomize(float value, float randomness, float minValue, float maxValue) {
            return Mathf.Max(minValue, Mathf.Min(maxValue, value + UnityEngine.Random.Range(-randomness, randomness)));
        }

    }
}
