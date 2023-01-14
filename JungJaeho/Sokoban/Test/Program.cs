
namespace Test
{
    class Program
    {
        public static int[] solution(int num, int total)
        {
            int n = total+num;
            int[] resultArr = new int[num];
            // total - n
            while (true)
            {
                int sum = 0;
                for (int i = 0; i < num; ++i)
                {
                    sum += n - i;
                }
                if (sum == total)
                    break;
                n--;
            }
            for (int i = 0; i < num; i++)
            {
                resultArr[i] = n - i;
            }
            Array.Sort(resultArr);
            return resultArr;
        }
        // -1 + 0 + 1
        public static void Main()
        {
            int[] sol = solution(3, 0);
            for (int i = 0; i < sol.Length; ++i)
                Console.WriteLine(sol[i]);
        }
    }
}