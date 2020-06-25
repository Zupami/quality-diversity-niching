using System.Collections;
using SharpNeat.Genomes.Neat;

namespace NoveltySearch.Individuals {
    public class BitIndividual : Individual {
        private BitArray _bitArray;
        private int _length = 30;

        public BitIndividual() {
            _bitArray = new BitArray(_length);
            for (int i = 0; i < _length; i++) {
                _bitArray[i] = RNG.NextBool();
            }
        }

        public BitIndividual(BitArray bitArray) {
            _bitArray = bitArray;
        }

        protected override double CalculateFitness() {
            double fitness = 0;
            for (int i = _length - 1; i >= 0; i--) {
                if (_bitArray[i]) {
                    fitness += 1;
                }
            }

            return fitness;
        }

        protected override double[] CalculateBehaviour() {
            throw new System.NotImplementedException();
        }

        public override double CalculateDistance(Individual individual) {
            throw new System.NotImplementedException();
        }

        public override Individual Mutate(uint gen) {
            BitArray bitArray = new BitArray(_length);
            double probability = 1d / _length;
            for (int i = 0; i < _length; i++) {
                if (RNG.Probability(probability)) {
                    bitArray[i] = !_bitArray[i];
                }
                else {
                    bitArray[i] = _bitArray[i];
                }
            }

            return new BitIndividual(bitArray);
        }

        public override Individual Combine(Individual individual, uint gen) {
            BitIndividual p1 = this;
            BitIndividual p2 = (BitIndividual)individual;
            BitArray bitArray = new BitArray(_length);
            for (int i = 0; i < _length; i++) {
                if (RNG.NextBool()) {
                    bitArray[i] = p1._bitArray[i];
                }
                else {
                    bitArray[i] = p2._bitArray[i];
                }
            }
            return new BitIndividual(bitArray);
        }

        public override bool IsMax() {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            string s = "";
            foreach (bool b in _bitArray) {
                s += b ? "1" : "0";
            }

            return s;
        }
    }
}
