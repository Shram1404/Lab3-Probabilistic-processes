using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
    internal class RandValuesStatisticCalculator
    {
        private readonly Random _rnd = new();

        public readonly double[] countArray = new double[6];
        public readonly double[] statistics = new double[4];
        public double[,] Table;
        public static string[] Labels;
        public double[] TableFirstCol;
        public double[] TableSecondCol;
        //Варіант 12
        private int N;
        private int[] _xi = { 7, 16, 28, 33, 39, 46, 56 };
        private double[] _pi = { 0.01, 0.05, 0.07, 0.1, 0.17, 0.25, 0.35 };
        private int[] _X;

        public RandValuesStatisticCalculator()
        {
            SetDiscreteRandValues(100, true);//<-- Змінити методи тут
            CalculateStatistics();
        }

        public void SetDiscreteRandValues(int n, bool isPuasson)
        {
            if(isPuasson)
            {
                _X = new int[n];
                this.N = n;
                double lambda = 12; // Варіант 12
                Random rand = new Random();
                for (int i = 0; i < n; i++)
                {
                    int y = GeneratePoisson(lambda, rand); //Генеруємо Пуассонівську випадкову величину з параметром λ
                    _X[i] = y;
                }

                double p = 0.1;
                double new_lambda = -Math.Log(p) * lambda; //Знаходимо нове λ зі зміненою P
                for (int i = 0; i < n; i++)
                {
                    int y = GeneratePoisson(new_lambda, rand); //Генеруємо нову Пуассонівську випадкову величину зі зміненим p
                    _X[i] = y;
                }

                GetDiscreteNumbersCount();
            }
            else
            {
                _X = new int[n];
                this.N = n;
                double[] P = new double[_pi.Length];

                P[0] = _pi[0];
                for (int i = 1; i < _pi.Length; i++)
                {
                    P[i] = P[i - 1] + _pi[i];
                }

                Random rand = new Random();
                for (int i = 0; i < n; i++)
                {
                    double r = rand.NextDouble();
                    int j = 0;
                    while (r >= P[j])
                    {
                        j++;
                    }
                    _X[i] = _xi[j];
                }
                GetDiscreteNumbersCount();
            }
            
        }

        //Обчислення параметра λ
        private double CalculateLambda(int[] xi, double[] pi)
        {
            double lambda = 0;
            for (int i = 0; i < xi.Length; i++)
            {
                lambda += xi[i] * pi[i];
            }
            return lambda;
        }

        //Генерування Пуассонівської випадкової величини з параметром λ
        private int GeneratePoisson(double lambda, Random rand)
        {
            double L = Math.Exp(-lambda);
            double p = 1.0;
            int k = 0;
            do
            {
                k++;
                p *= rand.NextDouble();
            } while (p > L);
            return k - 1;
        }

        private void GetDiscreteNumbersCount()
        {
            int numUniqueValues = _X.Distinct().Count();

            Table = new double[numUniqueValues, 3];

            int rowIndex = 0;
            foreach (int value in _X.Distinct())
            {
                int count = _X.Count(x => x == value);

                double frequency = (double)count / _X.Length;

                Table[rowIndex, 0] = value;
                Table[rowIndex, 1] = count;
                Table[rowIndex, 2] = (double)(frequency);
       
                rowIndex++;
            }
            int l = Table.GetLength(0);
            Labels = new string[l];

            TableFirstCol = new double[l];
            TableSecondCol = new double[l];

            for (int i = 0; i < l; i++)
            {
                TableFirstCol[i] = Table[i, 0];
                TableSecondCol[i] = Table[i, 1];
            }
            Array.Sort(TableFirstCol);
            for (int i = 0; i < l; i++)
            {
                Labels[i] = Convert.ToString(TableFirstCol[i]);
            }

        }

        public void CalculateStatistics()
        {
            //Вибіркове мат сподівання
            double mean = _X.Sum() / (double)_X.Length;
            statistics[0] = mean;

            //Вибіркова дисперсія
            double variance = _X.Sum(x => Math.Pow(x - mean, 2)) / (_X.Length - 1);
            statistics[1] = variance;
        }
    }
}
