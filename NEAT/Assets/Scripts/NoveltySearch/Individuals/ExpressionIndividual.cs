using System;
using System.Collections;
using SharpNeat.Genomes.Neat;

namespace NoveltySearch.Individuals {
    public class ExpressionIndividual : Individual {
        private BitArray _bitArray;
        private int _length = 60;
        public static double Goal = 2677;

        public ExpressionIndividual() {
            _bitArray = new BitArray(_length);
            for (int i = 0; i < _length; i++) {
                _bitArray[i] = RNG.NextBool();
            }
        }

        public ExpressionIndividual(BitArray bitArray) {
            _bitArray = bitArray;
        }

        protected override double CalculateFitness() {
            double fitness = 0;
            bool needDigit = true;
            Operator previousOperator = Operator.Plus;
            for (int i = 0; i < _length / 4; i++) {
                int value = 0;
                for (int j = 0; j < 4; j++) {
                    if (_bitArray[i * 4 + j]) {
                        value += (int)Math.Pow(2, j);
                    }
                }

                if (needDigit) {
                    if (value < 10) {
                        fitness = Operate(fitness, value, previousOperator);
                        needDigit = false;
                    }
                }
                else if (value >= 10 && value < 14) {
                    previousOperator = (Operator)(value - 10);
                    needDigit = true;
                }
            }

            fitness = Goal - Math.Abs(Goal - fitness);
            return (fitness < 0 || double.IsNaN(fitness) || double.IsInfinity(fitness)) ? 0 : fitness;
        }

        protected override double[] CalculateBehaviour() {
            double[] behaviour = new double[_length / 8 + 1];
            int k = 0;
            double fitness = 0;
            bool needDigit = true;
            Operator previousOperator = Operator.Plus;
            for (int i = 0; i < _length / 4; i++) {
                int value = 0;
                for (int j = 0; j < 4; j++) {
                    if (_bitArray[i * 4 + j]) {
                        value += (int)Math.Pow(2, j);
                    }
                }

                if (needDigit) {
                    if (value < 10) {
                        fitness = Operate(fitness, value, previousOperator);
                        if (fitness < 0 || double.IsNaN(fitness) || double.IsInfinity(fitness)) {
                            behaviour[k++] = 0;
                        }
                        else if (fitness > 2 * Goal) {
                            behaviour[k++] = 2 * Goal;
                        }
                        else {
                            behaviour[k++] = fitness;
                        }
                        needDigit = false;
                    }
                }
                else if (value >= 10 && value < 14) {
                    previousOperator = (Operator)(value - 10);
                    needDigit = true;
                }
            }

            for (; k < behaviour.Length; k++) {
                if (fitness < 0 || double.IsNaN(fitness) || double.IsInfinity(fitness)) {
                    behaviour[k] = 0;
                }
                else if (fitness > 2 * Goal) {
                    behaviour[k] = 2 * Goal;
                }
                else {
                    behaviour[k] = fitness;
                }
            }

            return behaviour;
        }

        public override double CalculateDistance(Individual individual) {
            throw new NotImplementedException();
        }

        private double Operate(double value1, double value2, Operator @operator) {
            switch (@operator) {
                case Operator.Plus:
                    return value1 + value2;
                case Operator.Minus:
                    return value1 - value2;
                case Operator.Times:
                    return value1 * value2;
                case Operator.Divide:
                    return Math.Floor(value1 / value2);
                /*case Operator.Power:
                    return Math.Pow(value1, value2);
                case Operator.Append:
                    return value1 > 0 ? value1 * 10 + value2 : value1 * 10 - value2;*/
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
            }
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

            return new ExpressionIndividual(bitArray);
        }

        public override Individual Combine(Individual individual, uint gen) {
            ExpressionIndividual p1 = this;
            ExpressionIndividual p2 = (ExpressionIndividual)individual;
            BitArray bitArray = new BitArray(_length);
            for (int i = 0; i < _length; i++) {
                if (RNG.NextBool()) {
                    bitArray[i] = p1._bitArray[i];
                }
                else {
                    bitArray[i] = p2._bitArray[i];
                }
            }
            return new ExpressionIndividual(bitArray);
        }

        public override bool IsMax() {
            return Goal - Fitness < 1;
        }

        public override string ToString() {
            string s = "";
            for (int i = 0; i < _length / 4; i++) {
                int value = 0;
                for (int j = 0; j < 4; j++) {
                    if (_bitArray[i * 4 + j]) {
                        value += (int)Math.Pow(2, j);
                    }
                }

                if (value < 10) {
                    s += value;
                }
                else if (value < 14) {
                    s += OperatorToString((Operator)(value - 10));
                }
                else {
                    s += "_";
                }
            }

            s += ": ";

            bool needDigit = true;
            Operator previousOperator = Operator.Plus;
            bool firstPlus = true;
            for (int i = 0; i < _length / 4; i++) {
                int value = 0;
                for (int j = 0; j < 4; j++) {
                    if (_bitArray[i * 4 + j]) {
                        value += (int)Math.Pow(2, j);
                    }
                }

                if (needDigit) {
                    if (value < 10) {
                        if (firstPlus) {
                            s += value;
                            firstPlus = false;
                        }
                        else {
                            s += OperatorToString(previousOperator) + value;
                        }
                        needDigit = false;
                    }
                    else {
                        s += "_";
                    }
                }
                else if (value >= 10 && value < 14) {
                    previousOperator = (Operator)(value - 10);
                    needDigit = true;
                }
                else {
                    s += "_";
                }
            }

            s += " = " + Fitness;

            return s;
        }

        private string OperatorToString(Operator @operator) {
            switch (@operator) {
                case Operator.Plus:
                    return "+";
                case Operator.Minus:
                    return "-";
                case Operator.Times:
                    return "*";
                case Operator.Divide:
                    return "/";
                /*case Operator.Power:
                    return "^";
                case Operator.Append:
                    return "|";*/
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
            }
        }
    }

    public enum Operator {
        Plus, Minus, Times, Divide//, Power, Append
    }
}
