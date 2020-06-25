using NoveltySearch.Containers;
using NoveltySearch.Individuals;
using NoveltySearch.Selections;
using NoveltySearch.Variations;

namespace NoveltySearch {
    public class GeneticAlgorithm {
        public Individual[] Population;
        public int PopulationSize;
        public Selection Selection;
        public Container Container;
        public Variation[] Variations;

        private Individual _best;

        public GeneticAlgorithm(int populationSize, Container container, Selection selection, Variation[] variations) {
            PopulationSize = populationSize;
            Population = new Individual[populationSize];
            Selection = selection;
            Container = container;
            Variations = variations;
        }

        public void Init<T>() where T : Individual, new() {
            for (int i = 0; i < PopulationSize; i++) {
                Population[i] = new T();
            }
            Container.Clear();
            _best = null;
        }

        public Individual PerformStep() {
            /*
             * Select from archive and previous generation
             * Mutate/Crossover
             * Evaluate fitness/descriptor
             * Add to archive
             * (Update scores like novelty, local competition, curiosity)
             */

            Population = Selection.Select(Population, Container);
            foreach (Variation variation in Variations) {
                Population = variation.Vary(Population, 0);
            }
            if (_best != null) {
                Population[0] = _best;
            }

            Individual best = Population[0];
            foreach (Individual individual in Population) {
                Container.Add(individual);
                if (individual.Fitness > best.Fitness) {
                    best = individual;
                }
            }
            _best = best;
            return best;
        }
    }
}
