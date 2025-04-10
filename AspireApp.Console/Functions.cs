using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal static class Functions
    {
        /// <summary>
        /// Optimizes the number of servers based on their power and cost.
        /// </summary>
        /// <param name="power"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public static int OptimizeServers(List<int> power, List<int> cost)
        {
            int result = 0;
            if (power.Count > 0 && cost.Count > 0 && power.Count == cost.Count)
            {
                for (int k = 0; k < power.Count - 1; k++)
                {
                    result++;
                    if ((power[k] < power[k + 1] && cost[k] > cost[k + 1]) || (power[k] > power[k + 1] && cost[k] < cost[k + 1]))
                    {
                        result--;
                    }
                }
                if ((power[power.Count - 1] < power[0] && cost[cost.Count - 1] > cost[0]) || (power[power.Count - 1] > power[0] && cost[cost.Count - 1] < cost[0]))
                {
                    result--;
                }
                else
                    result++;
            }
            return result;
        }

        /// <summary>
        /// Calculates the parking bill based on the entrance and leaving times.
        /// </summary>
        /// <param name="E"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static int ParkingBill(string E, string L)
        {
            const int entranceFee = 2, firstHourFee = 3, afterHourFee = 4;
            int result = 0;
            if (!string.IsNullOrEmpty(E) && !string.IsNullOrEmpty(L))
            {
                var timeE = E.Split(':').Select(int.Parse).ToArray();
                var timeL = L.Split(':').Select(int.Parse).ToArray();
                result += entranceFee;
                if (timeE.Length == 2 && timeL.Length == 2 && timeE[0] >= 0 && timeE[0] < 24 && timeE[1] >= 0 && timeE[1] < 60 && timeL[0] >= 0 && timeL[0] < 24 && timeL[1] >= 0 && timeL[1] < 60)
                {
                    int hours = timeL[0] - timeE[0];
                    int minutes = timeL[1] - timeE[1];

                    if (minutes > 0)
                        hours++;

                    result = entranceFee + (hours > 0 ? firstHourFee : 0) + afterHourFee * (hours - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the parity degree of a number.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int ParityDegree(int n)
        {
            int result = 0, power = 0;
            if (n > 0 && n <= 1000000000)
            {
                int k = 1;
                while (n >= k)
                {
                    if (n % k == 0)
                        result = power;

                    power++;
                    k = (int)Math.Pow(2, power);
                }
            }
            return result;
        }
    }
}
