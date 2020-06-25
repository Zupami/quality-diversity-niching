using NoveltySearch.Containers;
using NoveltySearch.Selections;
using NoveltySearch.Variations;

public struct EvolutionParameters {
    public Selection Selection;
    public Container Container;
    public Variation[] Variations;

    public EvolutionParameters(Selection selection, Container container, Variation[] variations) {
        Selection = selection;
        Container = container;
        Variations = variations;
    }
}
