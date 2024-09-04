using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.ServiceDefaults.Models
{
    public interface IPlotEngine
    {
        void SetUpLayout(PlotTemplate plotTemplate);
        void DrawLayout(PlotTemplate plotTemplate, SKPoint point);
        void DrawPlotTitle(PlotTemplate plotTemplate, SKPoint point, string addToTitle = "");
        void DrawAxis(PlotTemplate plotTemplate, SKPoint point);
        void DrawData(PlotTemplate plotTemplate, SKPoint point, PlotItem plotItem);
        void SetNoData(PlotTemplate plotTemplate, SKPoint point);
        void DrawPlots();
    }
}
