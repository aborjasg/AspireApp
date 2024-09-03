using static alglib;

namespace AspireApp.Libraries
{
    public static class Calculations
    {
        public static void Spline1dBuildLinear()
        {
            double[] x = new double[] { 0, 1, 2, 3 };
            double[] y = new double[] { 1, 5, 3, 9 };
            double[] y2 = new double[] { 1, 5, 3, 9, 0 };
            alglib.spline1dinterpolant s;

            alglib.spline1dbuildlinear(x, y, 4, out s);  // 'expert' interface is used
        }
    }
}
