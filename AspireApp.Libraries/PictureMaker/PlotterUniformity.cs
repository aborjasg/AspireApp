using AspireApp.Libraries.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.Libraries.PictureMaker
{
    public class PlotterUniformity : PlotBase, IPlotEngine
    {
        public PlotterUniformity() { }

        ///
        protected new void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
        {

        }

        public new void DrawData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem)
        {

        }
    }
}
