using System.Linq;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;
using NoveltySearch.Selections;

namespace NoveltySearch.Variations {
    public class RestrictedTournamentSelection : Selection {
        private int _w;

        public RestrictedTournamentSelection(int w) {
            _w = w;
        }

        public override Individual[] Select(Individual[] population, Container container) {
            if (population[0].Parents == null) {
                return population;
            }

            Individual[] newPopulation = population.Select(i => i.Parents[0]).ToArray();
            for (int i = 0; i < population.Length; i++) {
                Individual child = population[i];

                int closestIndex = RNG.Next(newPopulation.Length);
                Individual closest = newPopulation[closestIndex];
                closest.TempDistance = child.CalculateDistance(closest);
                for (int j = 1; j < _w; j++) {
                    int sampleIndex = RNG.Next(newPopulation.Length);
                    Individual sample = newPopulation[sampleIndex];
                    sample.TempDistance = child.CalculateDistance(sample);
                    if (sample.TempDistance < closest.TempDistance) {
                        closest = sample;
                        closestIndex = sampleIndex;
                    }
                }

                if (child.Fitness >= closest.Fitness) {
                    newPopulation[closestIndex] = child;
                }

            }

            return newPopulation;
        }

        public override string ToString() {
            return "restrictedtournamentselection";
        }
    }
}
