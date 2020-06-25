using System.Collections.Generic;
using System.Text;
using SharpNeat.Core;
using SharpNeat.DistanceMetrics;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using UnityEngine;

namespace NoveltySearch.Individuals {
    public class RobotIndividual : Individual {
        public Robot Robot;
        public NeatGenome Genome;
        private IDistanceMetric _distanceMetric;

        public RobotIndividual(Robot robot, Individual[] parents) {
            Robot = robot;
            Genome = robot.Genome;
            Parents = parents;
            _distanceMetric = new EuclideanDistanceMetric();
        }

        public RobotIndividual(NeatGenome genome) {
            Genome = genome;
            _distanceMetric = new EuclideanDistanceMetric();
        }

        protected override double CalculateFitness() {
            return 20 - Robot.GetGoalDistance();
        }

        protected override double[] CalculateBehaviour() {
            return new double[] {Robot.transform.position.x, Robot.transform.position.y};
        }

        public override double CalculateDistance(Individual individual) {
            return _distanceMetric.MeasureDistance(Genome.Position, ((RobotIndividual)individual).Genome.Position);
        }

        public override Individual Mutate(uint gen) {
            return new RobotIndividual(Genome.CreateOffspring(gen));
        }

        public override Individual Combine(Individual individual, uint gen) {
            NeatGenome genome = Genome.CreateOffspring(((RobotIndividual)individual).Genome, gen);
            if (CyclicNetworkTest.IsNetworkCyclic(genome)) {
                genome = Genome.CreateOffspring(((RobotIndividual)individual).Genome, gen);
                if (CyclicNetworkTest.IsNetworkCyclic(genome)) {
                    return Mutate(gen);
                }
            }
            return new RobotIndividual(genome);
        }

        public override bool IsMax() {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            StringBuilder s = new StringBuilder();
            foreach (KeyValuePair<ulong, double> coord in Genome.Position.CoordArray) {
                s.Append("[");
                s.Append(coord.Key);
                s.Append(";");
                s.Append(coord.Value.ToString("0.##"));
                s.Append("] ");
            }
            return s.ToString();
        }
    }
}
