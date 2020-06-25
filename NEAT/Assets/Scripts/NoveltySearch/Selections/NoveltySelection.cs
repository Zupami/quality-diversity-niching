using System;
using System.Linq;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;

namespace NoveltySearch.Selections {
    public class NoveltySelection : Selection {
        private int _k;

        public NoveltySelection(int k) {
            _k = k;
        }

        public override Individual[] Select(Individual[] population, Container container) {
            Individual[] containerIndividuals = container.GetIndividuals();
            double totalSparseness = 0;
            foreach (Individual individual in population) {
                foreach (Individual containerIndividual in containerIndividuals) {
                    containerIndividual.TempDistance = Helper.BehaviourDistance(individual, containerIndividual);
                }
                IOrderedEnumerable<Individual> orderedIndividuals = containerIndividuals.OrderBy(x => x.TempDistance);

                double sparseness = 0;
                int i = 0;
                foreach (Individual orderedIndividual in orderedIndividuals) {
                    sparseness += orderedIndividual.TempDistance;
                    if (++i >= _k) {
                        break;
                    }
                }
                individual.TempSparseness = sparseness / _k;
                totalSparseness += individual.TempSparseness;
            }

            Individual[] newPopulation = new Individual[population.Length];
            for (int i = 0; i < population.Length; i++) {
                newPopulation[i] = RandomSelect(population, totalSparseness);
            }

            return newPopulation;
        }

        private Individual RandomSelect(Individual[] population, double totalSparseness) {
            population = population.OrderBy(x => RNG.NextDouble()).ToArray();
            double target = RNG.NextDouble() * totalSparseness;
            double current = 0;
            foreach (Individual individual in population) {
                current += individual.TempSparseness;
                if (current >= target) {
                    return individual;
                }
            }

            return null;
        }

        public override string ToString() {
            return "noveltyselection";
        }
    }
}
