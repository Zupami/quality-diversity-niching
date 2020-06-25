using UnityEngine;

namespace NoveltySearch.Individuals {
    public abstract class Individual {
        private double _fitness;
        private bool _fitnessCalculated;
        private double[] _behaviour;
        private bool _behaviourCalculated;

        public double TempDistance;
        public double TempSparseness;
        public Individual[] Parents;

        public double Fitness {
            get {
                if (!_fitnessCalculated) {
                    _fitness = CalculateFitness();
                    _fitnessCalculated = true;
                }

                return _fitness;
            }
        }
        public double[] Behaviour {
            get {
                if (!_behaviourCalculated) {
                    _behaviour = CalculateBehaviour();
                    _behaviourCalculated = true;
                }

                return _behaviour;
            }
        }

        protected abstract double CalculateFitness();
        protected abstract double[] CalculateBehaviour();
        public abstract double CalculateDistance(Individual individual);
        public abstract Individual Mutate(uint gen);
        public abstract Individual Combine(Individual individual, uint gen);
        public abstract bool IsMax();
    }
}
