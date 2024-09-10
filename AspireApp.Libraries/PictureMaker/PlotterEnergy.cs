using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using MathNet.Numerics.Statistics;
using SkiaSharp;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspireApp.Libraries.PictureMaker
{
    public class PlotterEnergy : PlotBase, IPlotEngine
    {
        public PlotterEnergy()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        protected new void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
        { 
            switch (plotTemplate.PlotType)
            {
                case enmPlotType.heatmap:
                    {
                        plotTemplate.FrameSize = new float[] { Constants.NUM_COLS * plotTemplate.StrokeWidth, Constants.NUM_ROWS * plotTemplate.StrokeWidth };
                        
                        break;
                    }
                case enmPlotType.histogram2:
                    {  
                        if (plotTemplate.Bar!.Edges == null && AxisValues![0] != null)
                        {
                            AxisValues[0][0] = AxisValues[0][0] - Math.Round(AxisValues[0][2] / 2);
                            AxisValues[0][1] = AxisValues[0][1] + Math.Round(AxisValues[0][2] / 2);
                        }

                        break;
                    }
            }
            base.SetUpLayout(plotTemplate, plotItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        /// <param name="plotItem"></param>
        public new void DrawData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem)
        {
            SetUpLayout(plotTemplate, plotItem);
            base.DrawData(plotTemplate, point, surface, plotItem);
        }
    }
}
