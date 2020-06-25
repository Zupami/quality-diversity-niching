using System;
using NoveltySearch.Individuals;

namespace NoveltySearch {
    public static class Helper {
        public static double BehaviourDistance(Individual i1, Individual i2) {
            double[] b1 = i1.Behaviour;
            double[] b2 = i2.Behaviour;

            double distance = 0;
            for (int i = 0; i < b1.Length; i++) {
                double diff = Math.Abs(b1[i] - b2[i]);
                distance += diff;// * diff;
            }

            return Math.Sqrt(distance);
        }
    }
}
