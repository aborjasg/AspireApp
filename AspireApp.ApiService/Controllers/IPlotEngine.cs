using AspireApp.Libraries.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.ApiService
{
    public interface IPlotEngine
    {
        void SetUpLayout(PlotTemplate plotTemplate, DerivedData derivedData);
        void DrawLayout(PlotTemplate plotTemplate, SKPoint point, SKSurface surface);
        void DrawPlotTitle(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, string addToTitle = "");
        void DrawAxis(PlotTemplate plotTemplate, SKPoint point, SKSurface surface);
        void DrawData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem);                
    }
}
