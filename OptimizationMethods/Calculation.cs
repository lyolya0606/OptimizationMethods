using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using MoreLinq;

namespace OptimizationMethods {
    public class Calculation {
        private readonly double _H = 9;
        private readonly int _N = 10;
        private readonly double _alpha = 1;
        private readonly double _beta = 1;
        private readonly double _gamma = 1;
        private readonly double _minSumLAndS = 12;
        private readonly double _minL = 1;
        private readonly double _maxL = 15;
        private readonly double _minS = 1;
        private readonly double _maxS = 12;
        private readonly double _learningRate = 0.4;
        private readonly double _momentum = 0.2;
        private readonly double _startL = 9;
        private readonly double _startS = 11;


        private bool IsPointGood(double x, double y) {
            return ((y + x) >= _minSumLAndS);
        }
        public double Function(Pair pair) {
            double L = pair.X;
            double S = pair.Y;
            if (IsPointGood(L, S)) {
                return _alpha * Math.Pow((L - S), 2) + _beta / _H * Math.Pow((S + L - _gamma * _N), 2);
            }

            return double.NaN;
        }
        
        // частная производная по L
        private double PartialDerivativeL(Pair pair) {
            double L = pair.X;
            double S = pair.Y;
            return 2 * _alpha * (L - S) + 2 * _beta / _H * (S + L - _gamma * _N);
        }

        // частная производная по S
        private double PartialDerivativeS(Pair pair) {
            double L = pair.X;
            double S = pair.Y;
            return -2 * _alpha * (L - S) + 2 * _beta / _H * (S + L - _gamma * _N);
        }

        // градиент
        private Pair Gradient(Pair pair) {
            var gradient = new Pair(PartialDerivativeL(pair), PartialDerivativeS(pair));
            return gradient;
        }

        // градиентный спуск
        public List<Pair> GradientDescent() {
            List<Pair> listOfPairs = new();
            Pair currentPair = new(_startL, _startS);
            listOfPairs.Add(currentPair);
            Pair last;

            // while (true) {
            //     last = listOfPairs[listOfPairs.Count - 1];
            //     currentPair -= _learningRate * Gradient(currentPair);
            //     //currentPair = currentPair - _learningRate * Gradient(currentPair) + _momentum * (currentPair - last);
            //     // поменять на while 
            //     while (!IsPointGood(currentPair.X, currentPair.Y)) {
            //         currentPair = currentPair.Middle(last);
            //     }
            //     listOfPairs.Add(currentPair);
            //     if (Math.Abs(Function(currentPair) - Function(last)) < 0.01) {
            //         break;
            //     }
            // }
            
            
            for (int i = 0; i < 14; i++) {
                if (i == 0) {
                    last = listOfPairs[listOfPairs.Count - 1];
                } else {
                    last = listOfPairs[listOfPairs.Count - 2];
                }
            
                currentPair -= _learningRate * Gradient(currentPair);
                //currentPair = currentPair - _learningRate * Gradient(currentPair) + _momentum * (currentPair - last);
                // поменять на while 
                while (!IsPointGood(currentPair.X, currentPair.Y)) {
                    currentPair = currentPair.Middle(last);
                }
                listOfPairs.Add(currentPair);
            }

            return listOfPairs;
        }
        
        

        public List<double> GetL() {
            List<double> listOfL = new();
            double current = _minL;
            double step = 0.05;
            while (current <= _maxL) {
                listOfL.Add(current);
                current += step;
            }

            return listOfL;
        } 
        
        public List<double> GetS() {
            List<double> listOfS = new();
            double current = _minS;
            double step = 0.05;
            while (current <= _maxS) {
                listOfS.Add(current);
                current += step;
            }

            return listOfS;
        }

        public double[,] Result() {
            List<double> listOfL = GetL();
            List<double> listOfS = GetS();
            double[,] res = new double[listOfL.Count, listOfS.Count];
           // List<List<double>> res = new();
            // var selectMany = listOfL.SelectMany(x => listOfS, (x, y) => new { x, y });
            for (int i = 0; i < listOfL.Count; i++) {
                for (int j = 0; j < listOfS.Count; j++) {
                    res[i, j] = listOfL[i] * listOfS[j];
                }
            }
            return res;
        }
    }
}