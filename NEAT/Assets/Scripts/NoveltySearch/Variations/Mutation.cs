using System;
using NoveltySearch.Individuals;
using UnityEngine;

namespace NoveltySearch.Variations {
    public class Mutation : Variation {
        public override Individual[] Vary(Individual[] population, uint gen) {
            for (int i = 0; i < population.Length; i++) {
                Individual parent = population[i];
                Individual child = parent.Mutate(gen);
                child.Parents = new[] { parent };
                population[i] = child;
            }

            return population;
        }

        public override string ToString() {
            return "mutation";
        }
    }
}
