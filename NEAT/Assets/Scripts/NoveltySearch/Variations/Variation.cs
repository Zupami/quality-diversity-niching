using NoveltySearch.Individuals;

namespace NoveltySearch.Variations {
    public abstract class Variation {
        public abstract Individual[] Vary(Individual[] population, uint gen);
        public abstract override string ToString();
    }
}
