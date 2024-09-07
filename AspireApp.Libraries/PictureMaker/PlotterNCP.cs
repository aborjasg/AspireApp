using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.Libraries.PictureMaker
{
    public class PlotterNCP : PlotBase, IPlotEngine
    {
        public PlotterNCP()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        public new void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
        {
            // Get Scales:
            plotTemplate.FrameSize = new float[] { Constants.NUM_COLS * plotTemplate.StrokeWidth, Constants.NUM_ROWS * plotTemplate.StrokeWidth };
            // Set Axises:
            if (plotTemplate.Axis[0].Range != null && plotTemplate.Axis[0].Range.Length == 3)
            {
                AxisValues[0] = plotTemplate.Axis[0].Range;
            }
            if (plotTemplate.Axis[1].Range != null && plotTemplate.Axis[1].Range.Length == 3)
            {
                AxisValues[1] = plotTemplate.Axis[1].Range;
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

            if (plotItem.ArrayData != null)
            {
                // Preparing plot:
                SKBitmap bitmap = new SKBitmap((int)plotTemplate.FrameSize[0], (int)plotTemplate.FrameSize[1]);
                using var canvas = new SKCanvas(bitmap);
                var (x, y) = GetPoint0(plotTemplate);
                SKPoint pointRef = new SKPoint(x, y);                
                var color = SKColors.Black;
                var arrayData = plotItem.ArrayData!;
                var paintPoint = Constants.PaintPoint;
                paintPoint.StrokeWidth = plotTemplate.StrokeWidth;

                for (int col = 0; col < arrayData.GetLength(0); col++)
                {
                    for (int row = 0; row < arrayData.GetLength(1); row++)
                    {
                        if (arrayData[col, row] > 0)
                        {
                            var (px, py) = (col * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2, row * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2);
                            paintPoint.Color = color;
                            canvas.DrawPoint(new SKPoint(pointRef.X + px, pointRef.Y - py), paintPoint);
                        }
                    }
                }
                surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));
            }
            else
                SetNoData(plotTemplate, point, surface);

            base.DrawData(plotTemplate, point, surface, plotItem);
        }
    }
}
