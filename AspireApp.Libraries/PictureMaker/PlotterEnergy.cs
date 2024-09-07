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
            // Get ranges:
            if (plotTemplate.Axis[0].Range == null || plotTemplate.Axis[0].Range.Length == 0)
            {
                var range = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                plotTemplate.Axis[0].Range = new double[] { range[0], range[range.Length - 1], 1 };
            }
            if (plotTemplate.Axis[1].Range == null || plotTemplate.Axis[1].Range.Length == 0)
            {
                var range = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                plotTemplate.Axis[1].Range = new double[] { range[0], range[range.Length - 1], 1 };
            }

            switch (plotTemplate.PlotType)
            {
                case enmPlotType.heatmap:
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

                        break;
                    }
                case enmPlotType.histogram:
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
            if (plotItem.ArrayData != null)
            {               
                SKBitmap bitmap = new SKBitmap((int)plotTemplate.FrameSize[0], (int)plotTemplate.FrameSize[1]);
                using var canvas = new SKCanvas(bitmap);
                
                switch (plotTemplate.PlotType)
                {
                    case enmPlotType.heatmap:
                        {
                            SetUpLayout(plotTemplate, plotItem);

                            // Preparing plot:
                            var color = SKColors.Black;
                            var arrayData = plotItem.ArrayData!;
                            var paintPoint = Constants.PaintPoint;
                            paintPoint.StrokeWidth = plotTemplate.StrokeWidth;

                            if (plotTemplate.Bar != null)
                            {
                                switch (plotTemplate.Bar.ColorMap)
                                {
                                    case "viridis":
                                        {
                                            (plotTemplate.Bar.Colors, plotTemplate.Bar.ColorPositions) = ColorMap.Viridis;
                                            break;
                                        }
                                }
                          
                                for (int i = 0; i < arrayData.GetLength(0); i++)
                                {
                                    for (int j = 0; j < arrayData.GetLength(1); j++)
                                    {
                                        double factor = 1 - (plotTemplate.Bar!.Labels[1] - arrayData[i, j]) / (plotTemplate.Bar.Labels[1] - plotTemplate.Bar.Labels[0]);
                                        if (factor < 0)
                                            color = SKColors.Blue;
                                        else if (factor > 1)
                                            color = SKColors.Red;
                                        else
                                            color = ColorMap.GetColor((plotTemplate.Bar.Colors, plotTemplate.Bar.ColorPositions), (float)factor);

                                        var (px, py) = (i * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2, plotTemplate.FrameSize[1] - (j * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2));
                                        canvas.DrawPoint(new SKPoint(px, py), new SKPaint { Color = color, FilterQuality = SKFilterQuality.High, Style = SKPaintStyle.Fill, StrokeWidth = plotTemplate.StrokeWidth });
                                    }
                                }
                            }
                            else
                                Console.WriteLine("plotTemplate.Bar is invalid");
                                                        
                            break;
                        }
                    case enmPlotType.histogram:
                        {
                            var list = new List<double>();
                            var array1D = (double[])plotItem.ArrayData.To1D();
                            var tempMax = array1D.Max();
                            var tempMin = array1D.Min();
                            var tempMed = array1D.Median();
                            var tempDiff = Math.Min(Math.Abs(tempMax - tempMed), Math.Abs(tempMin - tempMed));

                            foreach (var elem in array1D)
                            {
                                if (tempDiff != 0)
                                {
                                    if (Math.Abs(tempMed - elem) < tempDiff * 10)
                                        list.Add((float)elem);
                                }
                                else
                                    list.Add((float)elem);
                            }
                                                        
                            var binSize = 10;
                            var bins = new double[] { 0, 1 };
                            double scope = (bins[1]) / binSize;
                            var dict = new Dictionary<double, int>();

                            for (double k = bins[0]; Math.Round(k, 4) <= bins[1]; k += Math.Round(scope, 4))
                            {
                                var count = list.Count(x => x >= k && x < k + scope);
                                dict.Add(Math.Round(k, 4), count);
                            }

                            // Axis X:
                            AxisValues[0] = new double[] { bins[0], bins[1], scope };
                            // Axis Y (auto-scaled if it's empty float[]):
                            const int totalMaxJumps = 6;
                            var (minY, maxY, baseY) = DataTransformation.AdjustLimits(dict.Values.Min(), dict.Values.Max(), totalMaxJumps, true);
                            AxisValues[1] = new double[] { 0, maxY, baseY };
                            SetUpLayout(plotTemplate, plotItem);

                            // Preparing plot:
                            float px, py0, py1;
                            SKColor color = SKColors.Green;
                            var paintPoint = new SKPaint { Color = color, FilterQuality = SKFilterQuality.High, StrokeWidth = plotTemplate.StrokeWidth };
                           
                            foreach (var elem in dict.OrderBy(x => x.Key))
                            {
                                var refX = (elem.Key - AxisValues![0][0]) * plotTemplate.Axis[0].Scale;
                                var refY = (elem.Value - AxisValues[1][0]) * plotTemplate.Axis[1].Scale;
                                px = (float)(refX + plotTemplate.Axis[0].Offset[0]);
                                py0 = (float)(plotTemplate.FrameSize[1] - plotTemplate.Axis[1].Offset[0]);
                                py1 = (float)(py0 - refY);

                                canvas.DrawLine(new SKPoint(px, py0), new SKPoint(px, py1), paintPoint);
                            }
                            break;
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
