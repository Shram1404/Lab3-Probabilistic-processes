using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class RandValuesStatisticCalculator
    {
        private List<int> _randValues= new();
        private readonly Random _rnd = new();

        public double[] countArray = new double[6];
        public double[] statistics = new double[4];

        public RandValuesStatisticCalculator() 
        {
            AddRandValues(10);
            CalculateStatistics(10);
        }

        public void AddRandValues(int value)
        {
            _randValues.Clear();
            _randValues = new List<int>();

            for (int i = 0; i < value; i++)
                _randValues.Add(_rnd.Next(0, 6));
            SetValuesCount();
        }

        private void SetValuesCount()
        {
            Dictionary<int, int> valueCount = new();
            foreach (int value in _randValues)
            {
                if (!valueCount.ContainsKey(value))
                    valueCount.Add(value, 0);
                valueCount[value]++;
            }

            int totalCount = _randValues.Count;

            for (int i = 0; i < countArray.Length; i++)
            {
                if (valueCount.ContainsKey(i))
                    countArray[i] = (double)valueCount[i] / totalCount;
                else
                    countArray[i] = 0;
            }
        }

        public double[] CalculateStatistics(int value)
        {
            double mean = _randValues.Sum() / (double)_randValues.Count;
            statistics[0] = mean;

            double variance = _randValues.Sum(x => Math.Pow(x - mean, 2)) / (_randValues.Count - 1);
            statistics[1] = variance;

            double stdDev = Math.Sqrt(variance);
            statistics[2] = stdDev;

            double log = Math.Log(value);
            statistics[3] = log;

            return statistics;
        }
    }
}
