using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class RandomManager : Singleton<RandomManager>
    {
        public RandomManager() 
        {
            random = new Random();
        }

        public int GetRandomInt()
        {
            return random.Next();
        }

        public int GetRandomInt(int maxValue)
        {
            return random.Next(maxValue + 1);
        }

        public int GetRandomRangeInt(int min, int max)
        {
            return random.Next(min, max+1);   
        }

        public double GetRandomDouble()
        {
            return random.NextDouble();
        }

        public double GetRandomRangeDouble(int min, int max)
        {
            int minNum = min;
            int maxNum = max;
            return min + random.NextDouble() * (maxNum - minNum);
        }

        public int GetRandomRangeDoubleToInt(int min, int max)
        {
            int minNum = min;
            int maxNum = max;
            return (int)(min + random.NextDouble() * (maxNum - minNum) + 0.5);
        }

        private Random random;
        
    }
}
