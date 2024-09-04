using AspireApp.ServiceDefaults.Shared;

namespace AspireApp.ServiceDefaults.Models
{
    public class PlotTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public int[] Index { get; set; } = []; // 0=X / 1=Y 
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public enmPlotType PlotType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public enmAreaLayout AreaLayout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float[] FrameSize { get; set; } = []; // 0=X / 1=Y 
        /// <summary>
        /// 
        /// </summary>
        public float[] FrameSpace { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public float StrokeWidth { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public PlotAxis[] Axis { get; set; } = []; // 0=X / 1=Y        
        /// <summary>
        /// 
        /// </summary>
        public PlotBar? Bar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PlotLegend? Legend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; } = true;
    }
}
