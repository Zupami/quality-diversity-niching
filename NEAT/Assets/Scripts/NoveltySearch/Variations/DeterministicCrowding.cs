using NoveltySearch.Individuals;

namespace NoveltySearch.Variations {
    public class DeterministicCrowding : Variation {
        public override Individual[] Vary(Individual[] population, uint gen) {
            for (int i = 0; i < population.Length; i++) {
                Individual parent = population[i];
                Individual child = parent.Mutate(gen);
                if (child.Fitness >= parent.Fitness) {
                    population[i] = child;
                }
            }

            return population;
        }

        public override string ToString() {
            return "deterministiccrowding";
        }
    }
}
