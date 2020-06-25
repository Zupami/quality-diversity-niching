using NoveltySearch.Individuals;
using SharpNeat.Network;
using UnityEngine;

namespace NoveltySearch.Variations {
    public class Crossover : Variation {
        public override Individual[] Vary(Individual[] population, uint gen) {
            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length; i++) {
                Individual p1 = population[RNG.Next(population.Length)];
                Individual p2 = population[RNG.Next(population.Length)];
                newPopulation[i] = p1.Combine(p2, gen);
                newPopulation[i].Parents = new[] {p1, p2};
            }

            return newPopulation;
        }

        public override string ToString() {
            return "crossover";
        }
    }
}
