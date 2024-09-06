using AspireApp.Libraries;
using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp.ApiService.Controllers
{
    internal class PlotterSpectrum : PlotBase, IPlotEngine
    {
        public PlotterSpectrum()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        public new void SetUpLayout(PlotTemplate plotTemplate, DerivedData derivedData)
        {
            // Get Scales:
            if (plotTemplate.Axis[0].Range == null || plotTemplate.Axis[0].Range.Length == 0)
            {
                var range = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                plotTemplate.Axis[0].Range = new double[] { range[0], range[range.Length - 1], 1 };
            }

            if (plotTemplate.Axis[1].Range == null || plotTemplate.Axis[1].Range.Length == 0)
            {
                var range = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                plotTemplate.Axis[1].Range = new double[] { range[0], range[range.Length - 1], 1 };
            }
            base.SetUpLayout(plotTemplate, derivedData);
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
                            surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));
                            break;
                        }
                }
            }
            else
                SetNoData(plotTemplate, point, surface);
        }
    }
}