using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NoveltySearch.Individuals;

namespace NoveltySearch.Containers {
    public class Grid : Container {
        private Individual[,] _individuals;
        private double _cellWidth, _cellHeight;
        private double _minX, _minY;
        private int _width, _height;

        public Grid(double minX, double maxX, double minY, double maxY, int width, int height) {
            _individuals = new Individual[width, height];
            _cellWidth = (maxX - minX) / width;
            _cellHeight = (maxY - minY) / height;
            _minX = minX;
            _minY = minY;
            _width = width;
            _height = height;
        }

        public override void Add(Individual newIndividual) {
            int x = (int)((newIndividual.Behaviour[0] - _minX) / _cellWidth);
            int y = (int)((newIndividual.Behaviour[1] - _minY)/ _cellHeight);
            if (x < 0 || x >= _width || y < 0 || y >= _height) {
                return;
            }
            Individual oldIndividual = _individuals[x, y];
            if (oldIndividual == null || newIndividual.Fitness > oldIndividual.Fitness) {
                _individuals[x, y] = newIndividual;
            }
        }

        public override Individual[] GetIndividuals() {
            List<Individual> individuals = new List<Individual>();
            foreach (Individual individual in _individuals) {
                if (individual != null) {
                    individuals.Add(individual);
                }
            }
            return individuals.ToArray();
        }

        public override void Clear() {
            _individuals = new Individual[_width, _height];
        }

        public override int GetAmount() {
            return _individuals.Cast<Individual>().Count(individual => individual != null);
        }

        public override string ToString() {
            return "grid";
        }
    }
}
