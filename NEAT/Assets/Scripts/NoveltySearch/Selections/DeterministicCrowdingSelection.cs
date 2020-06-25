using System.Collections;
using System.Collections.Generic;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;
using NoveltySearch.Selections;
using UnityEngine;

public class DeterministicCrowdingSelection : Selection
{
    public override Individual[] Select(Individual[] population, Container container) {
        if (population[0].Parents == null) {
            return population;
        }

        for (int i = 0; i < population.Length; i++) {
            Individual child = population[i];
            if (child.Parents[0].Fitness > child.Fitness) {
                population[i] = child.Parents[0];
            }
        }

        return population;
    }

    public override string ToString() {
        return "deterministiccrowdingselection";
    }
}
