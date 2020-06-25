using NoveltySearch.Individuals;

namespace NoveltySearch.Containers {
    public class DummyContainer : Container {
        public override void Add(Individual newIndividual) {
            
        }

        public override Individual[] GetIndividuals() {
            return new Individual[0];
        }

        public override void Clear() {
            
        }

        public override int GetAmount() {
            return 0;
        }

        public override string ToString() {
            return "dummycontainer";
        }
    }
}
