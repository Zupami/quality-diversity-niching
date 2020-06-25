using System;
using System.Collections.Generic;
using System.Linq;
using NoveltySearch.Individuals;

namespace NoveltySearch.Variations {
    public class Clearing : Variation {
        private double _sigma, _kappa;

        public Clearing(double sigma, double kappa) {
            _sigma = sigma;
            _kappa = kappa;
        }

        public override Individual[] Vary(Individual[] population, uint gen) {
            List<Individual> orderedPopulation = new List<Individual>();
            foreach (Individual individual in population) {
                orderedPopulation.Add(individual);
                orderedPopulation.Add(individual.Mutate(gen));
            }
            orderedPopulation = orderedPopulation.OrderByDescending(x => x.Fitness).ToList();
            List<Individual> trash = new List<Individual>();
            for (int i = 0; i < orderedPopulation.Count; i++) {
                int winners = 1;
                for (int j = i + 1; j < orderedPopulation.Count; j++) {
                    if (orderedPopulation[i].CalculateDistance(orderedPopulation[j]) < _sigma) {
                        if (winners < _kappa) {
                            winners++;
                        }
                        else {
                            trash.Add(orderedPopulation[j]);
                            orderedPopulation.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }

            if (orderedPopulation.Count >= population.Length) {
                for (int i = 0; i < population.Length; i++) {
                    population[i] = orderedPopulation[i];
                }
            }
            else {
                for (int i = 0; i < orderedPopulation.Count; i++) {
                    population[i] = orderedPopulation[i];
                }
                int left = population.Length - orderedPopulation.Count;
                for (int i = 0; i < left; i++) {
                    population[i + orderedPopulation.Count] = trash[i];
                }
            }

            return population;
        }

        public override string ToString() {
            return "clearing";
        }
    }
}
