using SkiaSharp;

namespace AspireApp.ServiceDefaults.Models
{
    public class PlotBar
    {
        /// <summary>
        /// 
        /// </summary>
        public int[] Point { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public int[] Size { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public float[] ColorPositions { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public SKColor[] Colors { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public string ColorMap { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public double[] Labels { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public float[] Offset { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public bool[] Edges { get; set; } = [];
    }
}
