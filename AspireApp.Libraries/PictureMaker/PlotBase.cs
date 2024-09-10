﻿using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using MathNet.Numerics.Statistics;
using SkiaSharp;
using System;
using System.Drawing;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace AspireApp.Libraries.PictureMaker
{
    public class PlotBase : IPlotEngine
    {
        /// <summary>
        /// 
        /// </summary>
        protected double[][] AxisValues { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PlotBase()
        {
            AxisValues = new double[][] { Array.Empty<double>(), Array.Empty<double>() };
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="plotItem"></param>
        protected void SetUpLayout(PlotTemplate plotTemplate, PlotItem plotItem)
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

            // Set Axises:
            if (plotTemplate.Axis[0].Range != null && plotTemplate.Axis[0].Range.Length == 3)
            {
                AxisValues[0] = plotTemplate.Axis[0].Range;
            }
            if (plotTemplate.Axis[1].Range != null && plotTemplate.Axis[1].Range.Length == 3)
            {
                AxisValues[1] = plotTemplate.Axis[1].Range;
            }
            SetScaleLayout(plotTemplate, plotItem);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="plotItem"></param>
        protected void SetScaleLayout(PlotTemplate plotTemplate, PlotItem plotItem)
        { 
            // Set Frame Size:
            if (plotTemplate.FrameSize.Length > 0)
            {
                double rangeX = 0, rangeY = 0, width = 0, height = 0;

                if (AxisValues![0].Length > 0)
                {
                    rangeX = AxisValues[0][1] - AxisValues[0][0];
                    width = plotTemplate.FrameSize[0] - plotTemplate.Axis[0].Offset[0] - plotTemplate.Axis[0].Offset[1];

                    if (rangeX != 0)
                        plotTemplate.Axis[0].Scale = width / rangeX;
                    else
                        plotTemplate.Axis[0].Scale = 1;
                }

                if (AxisValues![1].Length > 0)
                {
                    rangeY = AxisValues[1][1] - AxisValues[1][0];
                    height = plotTemplate.FrameSize[1] - plotTemplate.Axis[1].Offset[0] - plotTemplate.Axis[1].Offset[1];

                    if (rangeY != 0)
                        plotTemplate.Axis[1].Scale = height / rangeY;
                    else
                        plotTemplate.Axis[1].Scale = 1;
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        protected void DrawLayout(PlotTemplate plotTemplate, SKPoint point, SKSurface surface)
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        protected void DrawAxis(PlotTemplate plotTemplate, SKPoint point, SKSurface surface)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="surface"></param>
        /// <param name="edges"></param>
        protected void DrawBar(PlotTemplate plotTemplate, SKSurface surface, bool[]? edges = null)
        {
            if (plotTemplate.Bar != null && AxisValues != null)
            {
                if (plotTemplate.Bar.Active)
                {
                    var point = new SKPoint(plotTemplate.Bar.Point[0], plotTemplate.Bar.Point[1]);
                    var data = new uint[plotTemplate.Bar.Size[0], plotTemplate.Bar.Size[1]];

                    // right side gradient bar
                    var rcBar = new SKRect
                    {
                        Left = point.X,
                        Top = point.Y,
                        Right = point.X + data.GetLength(0),
                        Bottom = point.Y + data.GetLength(1)
                    };

                    //Canvas.DrawBitmap(bmp, point);
                    var paintBar = new SKPaint
                    {
                        Color = SKColors.Black,
                        IsAntialias = true,
                        StrokeCap = SKStrokeCap.Butt,
                        StrokeJoin = SKStrokeJoin.Miter,
                        StrokeWidth = 1,
                        Shader = SKShader.CreateLinearGradient(
                                new SKPoint(rcBar.Left, rcBar.Bottom),
                                new SKPoint(rcBar.Left, rcBar.Top),
                                plotTemplate.Bar.Colors,
                                plotTemplate.Bar.ColorPositions,
                                SKShaderTileMode.Repeat),
                        Style = SKPaintStyle.Fill
                    };

                    var paintBorder = new SKPaint
                    {
                        TextSize = 11f,
                        IsAntialias = true,
                        Color = SKColors.Black,
                        IsStroke = false,
                        StrokeWidth = 1f,
                        Style = SKPaintStyle.Stroke,
                        TextAlign = SKTextAlign.Right,
                        //Typeface = SKt
                    };

                    surface.Canvas.DrawRect(rcBar, paintBar);
                    surface.Canvas.DrawRect(rcBar, paintBorder);

                    // Addind 2 triangles:
                    if (edges == null) edges = new bool[] { true, true };

                    var paintTriangle = new SKPaint
                    {
                        StrokeCap = SKStrokeCap.Butt,
                        StrokeJoin = SKStrokeJoin.Miter,
                        Style = SKPaintStyle.Fill
                    };

                    if (edges[0])
                    {
                        var path1 = new SKPath();
                        path1.MoveTo(rcBar.Left, rcBar.Top);
                        path1.LineTo((rcBar.Right + rcBar.Left) / 2, rcBar.Top - rcBar.Width);
                        path1.LineTo(rcBar.Right, rcBar.Top);
                        path1.Close();

                        paintTriangle.Color = SKColors.Red;
                        surface.Canvas.DrawPath(path1, paintTriangle);
                        surface.Canvas.DrawPath(path1, paintBorder);
                    }

                    if (edges[1])
                    {
                        var path2 = new SKPath();
                        path2.MoveTo(rcBar.Left, rcBar.Bottom);
                        path2.LineTo((rcBar.Right + rcBar.Left) / 2, rcBar.Bottom + rcBar.Width);
                        path2.LineTo(rcBar.Right, rcBar.Bottom);
                        path2.Close();

                        paintTriangle.Color = SKColors.Blue;
                        surface.Canvas.DrawPath(path2, paintTriangle);
                        surface.Canvas.DrawPath(path2, paintBorder);
                    }

                    //label possition and label
                    string numDecimals = "0";
                    if (plotTemplate.Bar.Labels[2] < 1) numDecimals = plotTemplate.Bar.Labels[2] <= 0.25 ? "0.00" : "0.0";

                    paintBorder.Style = SKPaintStyle.StrokeAndFill;
                    var labels = new Dictionary<string, SKPoint>();
                    double jump = (plotTemplate.Bar.Size[1] - plotTemplate.Bar.Offset[0] - plotTemplate.Bar.Offset[1]) / (plotTemplate.Bar.Labels[1] - plotTemplate.Bar.Labels[0]);

                    for (double k = plotTemplate.Bar.Labels[0]; Math.Round(k, 4) <= plotTemplate.Bar.Labels[1]; k += Math.Round(plotTemplate.Bar.Labels[2], 4))
                    {
                        labels.Add(k.ToString(numDecimals), new SKPoint(rcBar.Right, (float)(rcBar.Bottom - plotTemplate.Bar.Offset[0] - (k - plotTemplate.Bar.Labels[0]) * jump)));
                    }

                    var rcMaxTextBound = new SKRect();
                    paintBorder.MeasureText("-5", ref rcMaxTextBound);
                    foreach (var item in labels)
                    {
                        surface.Canvas.DrawLine(item.Value, new SKPoint(item.Value.X + 5, item.Value.Y), paintBorder);
                        surface.Canvas.DrawText(item.Key, new SKPoint(item.Value.X + rcMaxTextBound.Width, item.Value.Y + rcMaxTextBound.Height / 2f - 2), Constants.PaintAxis);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        /// <param name="plotItem"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected void DrawFigures(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem)
        {
            if (plotItem.ArrayData != null)
            {
                // Preparing plot:
                SKBitmap bitmap = new SKBitmap((int)plotTemplate.FrameSize[0], (int)plotTemplate.FrameSize[1]);
                using var canvas = new SKCanvas(bitmap);
                const int seccionsForY = 6;

                switch (plotTemplate.PlotType)
                {
                    case enmPlotType.ncp:
                        {
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

                            break;
                        }
                    case enmPlotType.heatmap:
                        {
                            var (x, y) = GetPoint0(plotTemplate);
                            SKPoint pointRef = new SKPoint(x, y);
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
                                        canvas.DrawPoint(new SKPoint(pointRef.X + px, pointRef.Y - py), new SKPaint { Color = color, FilterQuality = SKFilterQuality.High, Style = SKPaintStyle.Fill, StrokeWidth = plotTemplate.StrokeWidth });
                                    }
                                }
                            }
                            surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));

                            break;
                        }
                    case enmPlotType.histogram1:
                        {
                            SetUpLayout(plotTemplate, plotItem);

                            // Preparing plot:
                            float px, py0, py1;
                            SKColor color = new SKColor(31, 119, 180);
                            var paintPoint = new SKPaint { Color = color, FilterQuality = SKFilterQuality.High, StrokeWidth = plotTemplate.StrokeWidth };
                            var arrX = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                            var arrY = (double[])plotItem.ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                                                        
                            var (minY, maxY, baseY) = DataTransformation.AdjustLimits(0, arrY.Max(), seccionsForY, true);
                            AxisValues[1] = new double[] { 0, maxY, baseY };
                            SetScaleLayout(plotTemplate, plotItem);

                            for (int k = 0; k < arrX.Length; k++)
                            {
                                var refX = (arrX[k] - AxisValues![0][0]) * plotTemplate.Axis[0].Scale;
                                var refY = (arrY[k] - AxisValues[1][0]) * plotTemplate.Axis[1].Scale;
                                px = (float)(refX + plotTemplate.Axis[0].Offset[0]);
                                py0 = (float)(plotTemplate.FrameSize[1] - plotTemplate.Axis[1].Offset[0]);
                                py1 = (float)(py0 - refY);

                                canvas.DrawLine(new SKPoint(px, py0), new SKPoint(px, py1), paintPoint);
                            }
                            surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));

                            break;
                        }
                    case enmPlotType.histogram2:
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
                            var (minY, maxY, baseY) = DataTransformation.AdjustLimits(dict.Values.Min(), dict.Values.Max(), seccionsForY, true);
                            AxisValues[1] = new double[] { 0, maxY, baseY };
                            SetScaleLayout(plotTemplate, plotItem);

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
                            surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));

                            break;
                        }
                    case enmPlotType.linechart:
                        {
                            SetUpLayout(plotTemplate, plotItem);

                            // Preparing plot:
                            var (x, y) = GetPoint0(plotTemplate);
                            SKPoint pointRef = new SKPoint(x, y);
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

                                surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));
                            }

                            break;
                        }
                }
            }
            else
                SetNoData(plotTemplate, point, surface);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        /// <param name="addToTitle"></param>
        public void DrawPlotTitle(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, string addToTitle = "")
        {
            surface.Canvas.DrawText(plotTemplate.Title + addToTitle, point.X + plotTemplate.FrameSize[0] / 2f, point.Y - plotTemplate.FrameSize[1] - 5, Constants.PaintTitle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        /// <param name="point"></param>
        /// <param name="surface"></param>
        /// <param name="plotItem"></param>
        public void DrawData(PlotTemplate plotTemplate, SKPoint point, SKSurface surface, PlotItem plotItem)
        {
            DrawFigures(plotTemplate, point, surface, plotItem);
            DrawLayout(plotTemplate, point, surface);
            DrawAxis(plotTemplate, point, surface);
            DrawBar(plotTemplate, surface);
        }

        #endregion
    }
}
