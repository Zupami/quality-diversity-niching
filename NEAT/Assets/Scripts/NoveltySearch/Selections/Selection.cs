using NoveltySearch.Containers;
using NoveltySearch.Individuals;

namespace NoveltySearch.Selections {
    public abstract class Selection {
        public abstract Individual[] Select(Individual[] population, Container container);
        public abstract override string ToString();
    }
}
