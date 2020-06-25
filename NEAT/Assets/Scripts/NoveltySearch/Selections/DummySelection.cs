using NoveltySearch.Containers;
using NoveltySearch.Individuals;

namespace NoveltySearch.Selections {
    public class DummySelection : Selection {
        public override Individual[] Select(Individual[] population, Container container) {
            return population;
        }

        public override string ToString() {
            return "dummyselection";
        }
    }
}
