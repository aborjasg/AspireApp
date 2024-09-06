using AspireApp.Libraries;
using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace AspireApp.ApiService.Controllers
{
    public class PlotBase: IPlotEngine
    {
        /// <summary>
        /// 
        /// </summary>
        protected double[][]? AxisValues { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PlotBase()
        {
        }

        #region Protected/Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <returns></returns>
        protected (float, float) GetPoint0(PlotTemplate plotTemplate)
        {
            float x = (float)(Math.Abs(plotTemplate.Axis[0].Range[0]) * plotTemplate.Axis[0].Scale);
            float y = plotTemplate.FrameSize[1] - (float)(Math.Abs(plotTemplate.Axis[1].Range[0]) * plotTemplate.Axis[1].Scale);

            return (x, y);
        }
        /// <summary>
        /// 
        /// </summary>
        protected void SetNoData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface)
        {
            surface.Canvas.DrawText("No data", new SKPoint(point.X + plotTemplate.FrameSize[0] / 2 - 20, point.Y - plotTemplate.FrameSize[1] / 2), Constants.PaintText);
        }

        #endregion


        #region Public Methods

        public void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
        {
            // Set Frame Size:
            if (plotTemplate.FrameSize.Length > 0)
            {
                double rangeX = 0, rangeY = 0, width = 0, height = 0;

                if (AxisValues!.GetLength(0) > 0)
                {
                    rangeX = AxisValues[0][1] - AxisValues[0][0];
                    rangeY = AxisValues[1][1] - AxisValues[1][0];

                    width = plotTemplate.FrameSize[0] - plotTemplate.Axis[0].Offset[0] - plotTemplate.Axis[0].Offset[1];
                    height = plotTemplate.FrameSize[1] - plotTemplate.Axis[1].Offset[0] - plotTemplate.Axis[1].Offset[1];

                    if (rangeX != 0)
                        plotTemplate.Axis[0].Scale = width / rangeX;
                    else
                        plotTemplate.Axis[0].Scale = 1;

                    if (rangeY != 0)
                        plotTemplate.Axis[1].Scale = height / rangeY;
                    else
                        plotTemplate.Axis[1].Scale = 1;
                }
            }
        }
        public void DrawLayout(PlotTemplate plotTemplate, SKPoint point, SKSurface surface)
        {
            var paintSquare = Constants.PaintSquare;

            switch (plotTemplate.AreaLayout)
            {
                case enmAreaLayout.Plain:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotTemplate.FrameSize[0], y0 = point.Y, y1 = point.Y - plotTemplate.FrameSize[1];

                        surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x0, y1), paintSquare);
                        surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x1, y0), paintSquare);
                        surface.Canvas.DrawLine(new SKPoint(x0, y1), new SKPoint(x1, y1), paintSquare);
                        surface.Canvas.DrawLine(new SKPoint(x1, y0), new SKPoint(x1, y1), paintSquare);

                        break;
                    }
                case enmAreaLayout.Squad:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotTemplate.FrameSize[0], y0 = point.Y - plotTemplate.FrameSize[1], y1 = point.Y;
                        float space = plotTemplate.StrokeWidth;
                        
                        // Dotted-line matrix:
                        for (float k = x0; k <= x1; k += space)
                        {
                            surface.Canvas.DrawLine(new SKPoint(k, y0), new SKPoint(k, y1), paintSquare);
                        }
                        for (float k = y0; k <= y1; k += space)
                        {
                            surface.Canvas.DrawLine(new SKPoint(x0, k), new SKPoint(x1, k), paintSquare);
                        }

                        paintSquare.PathEffect = null;

                        // bold-line matrix:
                        for (float k = x0; k <= x1; k += space * 3)
                        {
                            surface.Canvas.DrawLine(new SKPoint(k, y0), new SKPoint(k, y1), paintSquare);
                        }
                        for (float k = y0; k <= y1; k += space * 3)
                        {
                            surface.Canvas.DrawLine(new SKPoint(x0, k), new SKPoint(x1, k), paintSquare);
                        }
                        break;
                    }
            }
            DrawAxis(plotTemplate, point, surface);
        }
        public void DrawPlotTitle(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, string addToTitle = "")
        {
            surface.Canvas.DrawText(plotTemplate.Title + addToTitle, point.X + plotTemplate.FrameSize[0] / 2f, point.Y - plotTemplate.FrameSize[1] - 5, Constants.PaintTitle);
        }
        public void DrawAxis(PlotTemplate plotTemplate, SKPoint point, SKSurface surface)
        {
            SKPaint paint = Constants.PaintTextSmall.Clone();

            if (AxisValues != null)
            {
                string numDecimals = "0";
                if (AxisValues[0][2] < 1) numDecimals = AxisValues[0][2] <= 0.25 ? "0.00" : "0.0";

                // X Axis:
                paint.TextAlign = SKTextAlign.Center;

                for (double k = AxisValues[0][0]; k <= AxisValues[0][1]; k += AxisValues[0][2])
                {
                    var (x, y) = ((float)(point.X + plotTemplate.Axis[0].Offset[0] + (k - AxisValues[0][0]) * plotTemplate.Axis[0].Scale), point.Y);
                    var pointRef = new SKPoint(x, y);

                    surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X, pointRef.Y + 5), Constants.PaintBorder);
                    surface.Canvas.DrawText(k.ToString(numDecimals), new SKPoint(pointRef.X, pointRef.Y + 17), paint);
                }

                // Y Axis:
                paint.TextAlign = SKTextAlign.Right;

                for (double k = AxisValues[1][0]; k <= AxisValues[1][1]; k += AxisValues[1][2])
                {
                    var (x, y) = (point.X, (float)(point.Y - (plotTemplate.Axis[1].Offset[0] + (k - AxisValues[1][0]) * plotTemplate.Axis[1].Scale)));
                    var pointRef = new SKPoint(x, y);

                    surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X - 5, pointRef.Y), Constants.PaintBorder);
                    surface.Canvas.DrawText(k.ToString(), new SKPoint(pointRef.X - 7, pointRef.Y + 3), paint);
                }
            }
        }
        public void DrawData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem)
        {
            
        }

        #endregion
    }
}
