using System;
using System.Collections.Generic;
using NoveltySearch.Individuals;
using UnityEngine;

namespace NoveltySearch.Containers {
    public class Archive : Container {
        private List<Individual> _individuals = new List<Individual>();
        private double _threshold;

        public Archive(double threshold) {
            _threshold = threshold;
        }

        public override void Add(Individual newIndividual) {
            Tuple<Individual, double> tuple = GetClosest(newIndividual);
            Individual closest = tuple.Item1;
            double smallestDistance = tuple.Item2;

            if (closest == null) {
                _individuals.Add(newIndividual);
            }
            else {
                if (smallestDistance > _threshold) {
                    _individuals.Add(newIndividual);
                }
                else {
                    if (closest.Fitness < newIndividual.Fitness) {
                        double secondSmallestDistance = GetSecondClosest(newIndividual, closest).Item2;
                        if (secondSmallestDistance > _threshold) {
                            _individuals.Add(newIndividual);
                            _individuals.Remove(closest);
                        }
                    }
                }
            }
        }

        public override Individual[] GetIndividuals() {
            return _individuals.ToArray();
        }

        public override void Clear() {
            _individuals = new List<Individual>();
        }

        public override int GetAmount() {
            return _individuals.Count;
        }

        private Tuple<Individual, double> GetClosest(Individual newIndividual) {
            Individual closest = null;
            double smallestDistance = double.MaxValue;
            foreach (Individual individual in _individuals) {
                if (closest == null) {
                    closest = individual;
                    smallestDistance = Helper.BehaviourDistance(newIndividual, individual);
                }
                else {
                    double distance = Helper.BehaviourDistance(newIndividual, individual);
                    if (distance < smallestDistance) {
                        closest = individual;
                        smallestDistance = distance;
                    }
                }
            }
            return new Tuple<Individual, double>(closest, smallestDistance);
        }

        private Tuple<Individual, double> GetSecondClosest(Individual newIndividual, Individual closest) {
            Individual secondClosest = null;
            double secondSmallestDistance = double.MaxValue;
            foreach (Individual individual in _individuals) {
                if (individual == closest) {
                    continue;
                }
                if (secondClosest == null) {
                    secondClosest = individual;
                    secondSmallestDistance = Helper.BehaviourDistance(newIndividual, individual);
                }
                else {
                    double distance = Helper.BehaviourDistance(newIndividual, individual);
                    if (distance < secondSmallestDistance) {
                        secondClosest = individual;
                        secondSmallestDistance = distance;
                    }
                }
            }
            return new Tuple<Individual, double>(secondClosest, secondSmallestDistance);
        }

        public override string ToString() {
            return "archive";
        }
    }
}
