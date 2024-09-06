
using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using static alglib;

namespace AspireApp.Libraries
{
    public static class FakeData
    {
        public static double[,] GetNcpData()
        {
            var result = new double[Constants.NUM_COLS, Constants.NUM_ROWS];
            var random = new Random();

            for (int k = 0; k < 100; k++)
                result[random.Next(0, Constants.NUM_COLS), random.Next(0, Constants.NUM_ROWS)] = 1;

            return result;
        }

        public static double[,] GetLineChartData()
        {
            int n = 21; // # points
            double[] arrX = new double[n];
            for (int i = 0; i < n; i++)
                arrX[i] = (-n/2) + i;

            Func<double, double> Func0 = x => Math.Exp(x);
            var arrY0 = CalculateY(Func0, arrX);

            Func<double, double> Func1 = x => Math.Sin(x);
            var arrY1 = CalculateY(Func1, arrX);

            Func<double, double> Func2 = x => Math.Cos(x);
            var arrY2 = CalculateY(Func2, arrX);

            Func<double, double> Func3 = x => Math.Tan(x);
            var arrY3 = CalculateY(Func3, arrX);

            Func<double, double> Func4 = x => Math.Sqrt(x);
            var arrY4 = CalculateY(Func4, arrX);

            Func<double, double> Func5 = x => Math.Pow(x, 2);
            var arrY5 = CalculateY(Func5, arrX);

            var arrData = Array.CreateInstance(typeof(double), new int[] { 7, n });
            arrData.SetValue(arrX, new int?[] { 0, null });
            arrData.SetValue(arrY0, new int?[] { 1, null });
            arrData.SetValue(arrY1, new int?[] { 2, null });
            arrData.SetValue(arrY2, new int?[] { 3, null });
            arrData.SetValue(arrY3, new int?[] { 4, null });
            arrData.SetValue(arrY4, new int?[] { 5, null });
            arrData.SetValue(arrY5, new int?[] { 6, null });

            return (double[,])arrData;
        }

        public static double[,] GetHistogramData()
        {
            int n = 20;
            var arrX = new int[n];
            var arrY = new int[n];
            var random = new Random();

            var list = new List<int>();
            for (int i = 0; i < n; i++)
            {
                arrX[i] = i;
                arrY[i] = random.Next(0, 100);
            }
            
            var arrData = Array.CreateInstance(typeof(double), new int[] { 2, n });
            arrData.SetValue(arrX, new int?[] { 0, null });
            arrData.SetValue(arrY, new int?[] { 1, null });
            return (double[,])arrData;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Spline1dBuildLinear()
        {
            double[] x = new double[] { 0, 1, 2, 3 };
            double[] y = new double[] { 1, 5, 3, 9 };
            double[] y2 = new double[] { 1, 5, 3, 9, 0 };
            alglib.spline1dinterpolant s;

            alglib.spline1dbuildlinear(x, y, 4, out s);  // 'expert' interface is used
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double[] LeastSquares(double[,] a, double[] b)
        {
            var result = new double[0];
            alglib.lsfitreport rep;

            alglib.lsfitlinear(b, a, 11, 1, out result, out rep);
            return result;
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double[] CalculateY (Func<double, double> function, double[] x )
        {
            double[] y = new double[x.Length];
            for (int i = 0; i < y.Length; i++)
                y[i] = function(x[i]);
            return y;
        }
    }
}
