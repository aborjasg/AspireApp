
using static alglib;

namespace AspireApp.Libraries
{
    public static class Calculations
    {
        public delegate double MyFunction(double x);

        public static double[,] GetChartData()
        {
            int n = 21; // # points
            double[] arrX = new double[n];
            for (int i = 0; i < n; i++)
                arrX[i] = (-n/2) + i;

            Func<double, double> Func0 = x => Math.Atan(x);
            var arrY0 = CalculateY(Func0, arrX);

            Func<double, double> Sin = x => Math.Sin(x);
            var arrY1 = CalculateY(Sin, arrX);

            Func<double, double> Cos = x => Math.Cos(x);
            var arrY2 = CalculateY(Cos, arrX);

            Func<double, double> Pow = x => Math.Pow(x, 2);
            var arrY3 = CalculateY(Pow, arrX);

            var arrData = Array.CreateInstance(typeof(double), new int[] { 5, n });
            arrData.SetValue(arrX, new int?[] { 0, null });
            arrData.SetValue(arrY0, new int?[] { 1, null });
            arrData.SetValue(arrY1, new int?[] { 2, null });
            arrData.SetValue(arrY2, new int?[] { 3, null });
            arrData.SetValue(arrY3, new int?[] { 4, null });

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
