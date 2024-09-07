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
    public class PlotterSpectrum : PlotBase, IPlotEngine
    {
        public PlotterSpectrum()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        public  void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
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
                case enmPlotType.linechart:
                    {
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
                        var arrX = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                        var arrY = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);

                        // Axis X  (auto-scaled if it's empty float[]):
                        int jumps = 5;
                        var (minX, maxX, baseX) = DataTransformation.AdjustLimits(arrX.Min(), arrX.Max(), jumps, true);
                        AxisValues[0] = new double[] { minX, maxX, baseX };

                        // Axis Y (auto-scaled if it's empty float[]):
                        var (minY, maxY, baseY) = DataTransformation.AdjustLimits(arrY.Min(), arrY.Max(), jumps, true);
                        AxisValues[1] = new double[] { minY, maxY, baseY };
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
                SetUpLayout(plotTemplate, plotItem);

                // Preparing plot:
                var (x, y) = GetPoint0(plotTemplate);
                SKPoint pointRef = new SKPoint(x, y);
                SKBitmap bitmap = new SKBitmap((int)plotTemplate.FrameSize[0], (int)plotTemplate.FrameSize[1]);
                using var canvas = new SKCanvas(bitmap);

                switch (plotTemplate.PlotType)
                {
                    case enmPlotType.linechart:
                        {
                            var arrX = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                            float px, py;
                            SKPath path;

                            for (int j = 1; j < plotItem.ArrayData.GetLength(0); j++) // Start from index 1 the n curves for Y axis
                            {
                                var arrY = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(j), null }!);
                                path = new SKPath();
                                path.MoveTo(pointRef);

                                for (int i = 0; i < arrX.Length; i++)
                                {
                                    px = (float)(pointRef.X + arrX[i] * plotTemplate.Axis[0].Scale);
                                    py = (float)(pointRef.Y - arrY[i] * plotTemplate.Axis[1].Scale);
                                    if (i == 0)
                                        path.MoveTo(px, py);
                                    else
                                        path.LineTo(px, py);
                                }

                                // Draw into canvas:
                                var paint = new SKPaint()
                                {
                                    Color = Constants.Brushes[j - 1],
                                    IsAntialias = false,
                                    IsStroke = false,
                                    StrokeWidth = 1f,
                                    Style = SKPaintStyle.Stroke
                                };

                                canvas.DrawPath(path, paint);
                                canvas.DrawPoint(pointRef, Constants.PaintBack);
                                path.Close();
                            }
                            break;
                        }
                    case enmPlotType.histogram:
                        {
                            // Preparing plot:
                            float px, py0, py1;
                            SKColor color = new SKColor(31, 119, 180);
                            var paintPoint = new SKPaint { Color = color, FilterQuality = SKFilterQuality.High, StrokeWidth = plotTemplate.StrokeWidth };
                            var arrX = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                            var arrY = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);

                            for (int k = 0; k < arrX.Length; k++)
                            {
                                var refX = (arrX[k] - AxisValues![0][0]) * plotTemplate.Axis[0].Scale;
                                var refY = (arrY[k] - AxisValues[1][0]) * plotTemplate.Axis[1].Scale;
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