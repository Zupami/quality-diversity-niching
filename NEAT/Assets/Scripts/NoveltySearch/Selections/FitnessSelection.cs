using System.Linq;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;

namespace NoveltySearch.Selections {
    public class FitnessSelection : Selection {
        public override Individual[] Select(Individual[] population, Container container) {
            double totalFitness = 0;
            foreach (Individual individual in population) {
                totalFitness += individual.Fitness;
            }

            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length; i++) {
                newPopulation[i] = RandomSelect(population, totalFitness);
            }

            return newPopulation;
        }

        private Individual RandomSelect(Individual[] population, double totalFitness) {
            population = population.OrderBy(x => RNG.NextDouble()).ToArray();
            double target = RNG.NextDouble() * totalFitness;
            double current = 0;
            foreach (Individual individual in population) {
                current += individual.Fitness;
                if (current >= target) {
                    return individual;
                }
            }

            return null;
        }

        public override string ToString() {
            return "fitnessselection";
        }
    }
}
