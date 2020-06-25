using NoveltySearch.Containers;
using NoveltySearch.Individuals;

namespace NoveltySearch.Selections {
    public class ContainerSelection : Selection {
        public override Individual[] Select(Individual[] population, Container container) {
            Individual[] containerIndividuals = container.GetIndividuals();
            if (containerIndividuals.Length == 0) {
                return population;
            }
            for (int i = 0; i < population.Length; i++) {
                population[i] = containerIndividuals[RNG.Next(containerIndividuals.Length)];
            }

            return population;
        }

        public override string ToString() {
            return "containerselection";
        }
    }
}
