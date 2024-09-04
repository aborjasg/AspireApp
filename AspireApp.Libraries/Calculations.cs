
using static alglib;

namespace AspireApp.Libraries
{
    public static class Calculations
    {
        public delegate double MyFunction(double x);

        public static double[,] GetChartData()
        {            
            double[] x = new double[10];
            for (int i = 0; i < 10; i++)
                x[i] = i;

            Func<double, double> square = x => Math.Sqrt(x);
            var y = CalculateY(square, x);
            //Console.WriteLine(alglib.ap.format(y, 4));
            //var arrData = new double[,] { Array.Empty<double>(), new double[] { } };
            var arrData = Array.CreateInstance(typeof(double), new int[] { 2, 10 });
            arrData.SetValue(x, new int?[] { 0, null });
            arrData.SetValue(y, new int?[] { 1, null });
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
