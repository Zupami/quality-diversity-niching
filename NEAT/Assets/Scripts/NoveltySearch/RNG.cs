using System;

namespace NoveltySearch {
    public static class RNG {
        private static Random Random;

        public static void Init() {
            Random = new Random();
        }

        public static void Init(int seed) {
            Random = new Random(seed);
        }

        public static int Next() {
            return Random.Next();
        }

        public static int Next(int maxValue) {
            return Random.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue) {
            return Random.Next(minValue, maxValue);
        }

        public static double NextDouble() {
            return Random.NextDouble();
        }

        public static bool NextBool() {
            return Random.Next(2) == 0;
        }

        public static bool Probability(double probability) {
            return Random.NextDouble() < probability;
        }
    }
}
