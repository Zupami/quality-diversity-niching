using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;
using NoveltySearch.Selections;
using UnityEngine;

public class ClearingSelection : Selection 
{
    private double _sigma, _kappa;

    public ClearingSelection(double sigma, double kappa) {
        _sigma = sigma;
        _kappa = kappa;
    }

    public override Individual[] Select(Individual[] population, Container container) {
        List<Individual> orderedPopulation = population.OrderByDescending(x => x.Fitness).ToList();
        for (int i = 0; i < orderedPopulation.Count; i++) {
            int winners = 1;
            for (int j = i + 1; j < orderedPopulation.Count; j++) {
                if (orderedPopulation[i].CalculateDistance(orderedPopulation[j]) < _sigma) {
                    if (winners < _kappa) {
                        winners++;
                    }
                    else {
                        orderedPopulation.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        for (int i = 0; i < population.Length; i++) {
            foreach (Individual individual in orderedPopulation) {
                population[i] = individual;
                i++;
                if (i >= population.Length) {
                    break;
                }
            }
        }

        return population;
    }

    public override string ToString() {
        return "clearingselection" + _sigma + "." + _kappa;
    }
}
