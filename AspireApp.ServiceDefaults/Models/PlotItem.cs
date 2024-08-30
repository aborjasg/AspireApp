namespace AspireApp.ServiceDefaults.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PlotItem
    {
        public string Name { get; set; } = string.Empty;
        public double[,]? ArrayData { get; set; }
        public int[] PointRef { get; set; } = new int[2];
        public int[] IndexRef { get; set; } = new int[2];
    }
}
