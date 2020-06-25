using System.Linq;
using NoveltySearch.Individuals;

namespace NoveltySearch.Containers {
    public abstract class Container {
        public abstract void Add(Individual newIndividual);
        public abstract Individual[] GetIndividuals();
        public abstract void Clear();
        public abstract int GetAmount();
        public abstract override string ToString();
    }
}
