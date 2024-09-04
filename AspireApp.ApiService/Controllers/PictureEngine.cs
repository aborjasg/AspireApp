using AspireApp.Libraries;
using AspireApp.ServiceDefaults.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspireApp.ApiService.Controllers
{
    public class PictureEngine: IPictureEngine
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected Guid Guid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected PictureTemplate pictureTemplate;
        /// <summary>
        /// 
        /// </summary>
        protected DerivedData derivedData; 
        /// <summary>
        /// 
        /// </summary>
        protected string[]? PictureLegend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected double[][]? AxisValues { get; set; }

        #endregion

        #region Plot Objects

        /// <summary>
        /// 
        /// </summary>
        protected SKImageInfo ImageInfo;
        /// <summary>
        /// 
        /// </summary>
        protected SKSize PictureSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SKSurface Surface { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintTextSmall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintTextSmallCenter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintAxis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintLegend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintFrame { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintFrameSmall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintBorder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected SKPaint PaintPoint { get; set; }
        
        #endregion "Plot Objects"

        public PictureEngine(PictureTemplate pictureTemplate, DerivedData derivedData)
        {
            Guid = Guid.NewGuid();
            this.pictureTemplate = pictureTemplate;
            ImageInfo = new SKImageInfo(pictureTemplate.PictureDimensions[0], pictureTemplate.PictureDimensions[1]);
            Surface = SKSurface.Create(ImageInfo);
            Surface.Canvas.Clear(SKColors.White);
            
            PaintTitle = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = false,
                IsStroke = false,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                TextSize = 11f,
                FilterQuality = SKFilterQuality.High
            };
            PaintText = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = false,
                IsStroke = false,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                TextSize = 14f,
                FilterQuality = SKFilterQuality.High
            };
            PaintTextSmall = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = false,
                IsStroke = false,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                TextSize = 10f,
                FilterQuality = SKFilterQuality.High
            };
            PaintTextSmallCenter = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = false,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 9f,
                FilterQuality = SKFilterQuality.High
            };
            PaintAxis = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = false,
                IsStroke = true,
                StrokeWidth = 1f,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                TextSize = 10f,
                FilterQuality = SKFilterQuality.High
            };
            PaintLegend = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = true,
                IsStroke = true,
                StrokeWidth = 1f,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                TextSize = 9f,
                FilterQuality = SKFilterQuality.High
            };
            PaintBack = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = true,
                IsStroke = true,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 2,
                FilterQuality = SKFilterQuality.High
            };
            PaintFrame = new SKPaint()
            {
                Color = SKColors.WhiteSmoke,
                IsAntialias = true,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                TextAlign = SKTextAlign.Left,
                FilterQuality = SKFilterQuality.High
            };
            PaintFrameSmall = new SKPaint()
            {
                Color = SKColors.Black,
                IsAntialias = true,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Stroke,
                Typeface = SKTypeface.FromFamilyName("Arial"),
                TextAlign = SKTextAlign.Left,
                FilterQuality = SKFilterQuality.High
            };
            PaintBorder = new SKPaint()
            {
                TextSize = 11f,
                IsAntialias = false,
                IsStroke = true,
                Color = SKColors.Black,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Stroke,
                TextAlign = SKTextAlign.Right,
                FilterQuality = SKFilterQuality.High
            };
            PaintPoint = new SKPaint()
            {
                TextSize = 11f,
                IsAntialias = false,
                IsStroke = true,
                Color = SKColors.Black,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Stroke,
                TextAlign = SKTextAlign.Right,
                FilterQuality = SKFilterQuality.High
            };
            this.derivedData = derivedData;
        }

        /// <summary>
        /// 
        /// </summary>
        protected double[][] GetScaledAxisValues(PlotTemplate plotTemplate)
        {
            var result = new double[][] { Array.Empty<double>(), Array.Empty<double>() };

            if (plotTemplate.Axis != null)
            {
                if (plotTemplate.Axis[0].Range != null && plotTemplate.Axis[0].Range.Length == 3)
                {
                    result[0] = plotTemplate.Axis[0].Range;
                }
                if (plotTemplate.Axis[1].Range != null && plotTemplate.Axis[1].Range.Length == 3)
                {
                    result[1] = plotTemplate.Axis[1].Range;
                }
            }
            else
            {
                var arrX = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                var arrY = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                
                // X axis:
                int totalMaxJumps = 10;
                var (minX, maxX, baseX) = DataTransformation.AdjustLimits(0, arrX.Length, totalMaxJumps, true);
                result[0] = new double[] { minX, maxX, baseX };

                // Y axis:
                totalMaxJumps = 6;
                var (minY, maxY, baseY) = DataTransformation.AdjustLimits(arrY.Min(), arrY.Max(), totalMaxJumps, true);
                result[1] = new double[] { minY, maxY, baseY };
            }
            return result;
        }

        protected (float, float) GetPoint0(PlotTemplate plotTemplate)
        {
            float x = (float)(Math.Abs(plotTemplate.Axis[0].Range[0]) * plotTemplate.Axis[0].Scale);
            float y = plotTemplate.FrameSize[1] - (float)(Math.Abs(plotTemplate.Axis[1].Range[0]) * plotTemplate.Axis[1].Scale);

            return (x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotTemplate"></param>
        protected void SetUpLayout(PlotTemplate plotTemplate)
        {
            // Set Frame Size:
            if (plotTemplate.FrameSize.Length > 0)
            {
                double rangeX = 0, rangeY = 0, width = 0, height = 0;

                switch (plotTemplate.PlotType)
                {                    
                    case enmPlotType.ncp:
                        {
                            // Get Scales:
                            plotTemplate.FrameSize = new float[] { Constants.NUM_COLS * plotTemplate.StrokeWidth, Constants.NUM_ROWS * plotTemplate.StrokeWidth };
                            PaintPoint.StrokeWidth = plotTemplate.StrokeWidth;             
                                                        
                            break;
                        }
                    case enmPlotType.linechart:
                        {
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
                            break;
                        }
                }

                AxisValues = GetScaledAxisValues(plotTemplate);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="point"></param>
        /// <param name="PlotTemplate"></param>
        protected void DrawLayout(PlotTemplate plotTemplate, SKPoint point)
        {
            SetUpLayout(plotTemplate);

            var paintSquare = new SKPaint()
            {
                Color = SKColors.Gray,
                IsAntialias = false,
                IsStroke = true,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Miter,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 1f,
                PathEffect = SKPathEffect.CreateDash(new float[] { 1, 1 }, 2)
            };

            switch (plotTemplate.AreaLayout)
            {
                case enmAreaLayout.Plain:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotTemplate.FrameSize[0], y0 = point.Y, y1 = point.Y - plotTemplate.FrameSize[1];
                                                                       
                        Surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x0, y1), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x1, y0), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x0, y1), new SKPoint(x1, y1), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x1, y0), new SKPoint(x1, y1), paintSquare);

                        break;
                    }
                case enmAreaLayout.Squad:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotTemplate.FrameSize[0], y0 = point.Y - plotTemplate.FrameSize[1], y1 = point.Y;
                        float space = plotTemplate.StrokeWidth;

                        // Dotted-line matrix:
                        for (float k = x0; k <= x1; k += space)
                        {
                            Surface.Canvas.DrawLine(new SKPoint(k, y0), new SKPoint(k, y1), paintSquare);
                        }
                        for (float k = y0; k <= y1; k += space)
                        {
                            Surface.Canvas.DrawLine(new SKPoint(x0, k), new SKPoint(x1, k), paintSquare);
                        }

                        paintSquare.PathEffect = null;

                        // bold-line matrix:
                        for (float k = x0; k <= x1; k += space * 3)
                        {
                            Surface.Canvas.DrawLine(new SKPoint(k, y0), new SKPoint(k, y1), paintSquare);
                        }
                        for (float k = y0; k <= y1; k += space * 3)
                        {
                            Surface.Canvas.DrawLine(new SKPoint(x0, k), new SKPoint(x1, k), paintSquare);
                        }
                        break;
                    }                
            }            
            DrawAxis(plotTemplate, point);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawPlotTitle(PlotTemplate plotTemplate, SKPoint point, string addToTitle = "")
        {
            Surface.Canvas.DrawText(plotTemplate.Title + addToTitle, point.X + plotTemplate.FrameSize[0] / 2f, point.Y - plotTemplate.FrameSize[1] - 5, PaintTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotName"></param>
        /// <param name="plotType"></param>
        /// <param name="info"></param>
        /// <param name="point"></param>
        /// <param name="subtitle"></param>
        /// <param name="args"></param>
        protected void DrawAxis(PlotTemplate plotTemplate, SKPoint point)
        {
            SKPaint paint = PaintTextSmall.Clone();

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

                    Surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X, pointRef.Y + 5), PaintBorder);
                    Surface.Canvas.DrawText(k.ToString(numDecimals), new SKPoint(pointRef.X, pointRef.Y + 17), paint);
                }

                // Y Axis:
                paint.TextAlign = SKTextAlign.Right;

                for (double k = AxisValues[1][0]; k <= AxisValues[1][1]; k += AxisValues[1][2])
                {
                    var (x, y) = (point.X, (float)(point.Y - (plotTemplate.Axis[1].Offset[0] + (k - AxisValues[1][0]) * plotTemplate.Axis[1].Scale)));
                    var pointRef = new SKPoint(x, y);

                    Surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X - 5, pointRef.Y), PaintBorder);
                    Surface.Canvas.DrawText(k.ToString(), new SKPoint(pointRef.X - 7, pointRef.Y + 3), paint);
                }
            }
        }

        protected void DrawData(PlotTemplate plotTemplate, SKPoint point, PlotItem plotItem)
        {
            if (plotItem.ArrayData != null)
            {
                var (x, y) = GetPoint0(plotTemplate);
                SKPoint pointRef = new SKPoint(x, y);
                SKBitmap bitmap = new SKBitmap((int)plotTemplate.FrameSize[0], (int)plotTemplate.FrameSize[1]);
                using var canvas = new SKCanvas(bitmap);

                switch (plotTemplate.PlotType)
                {
                    case enmPlotType.ncp:
                        {
                            // Preparing plot:        
                            var color = SKColors.Black;
                            var arrayData = (double[,])plotItem.ArrayData;

                            for (int col = 0; col < arrayData.GetLength(0); col++)
                            {
                                for (int row = 0; row < arrayData.GetLength(1); row++)
                                {
                                    if (arrayData[col, row] > 0)
                                    {
                                        var (px, py) = (col * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2, row * plotTemplate.StrokeWidth + plotTemplate.StrokeWidth / 2);
                                        PaintPoint.Color = color;
                                        canvas.DrawPoint(new SKPoint(pointRef.X + px, pointRef.Y - py), PaintPoint);
                                    }
                                }
                            }
                            Surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));
                            break;
                        }
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
                                    Color = Constants.Brushes[j-1],
                                    IsAntialias = false,
                                    IsStroke = false,
                                    StrokeWidth = 1f,
                                    Style = SKPaintStyle.Stroke
                                };

                                canvas.DrawPath(path, paint);
                                canvas.DrawPoint(pointRef, PaintBack);
                                path.Close();
                            }
                            Surface.Canvas.DrawBitmap(bitmap, new SKPoint(point.X, point.Y - (int)plotTemplate.FrameSize[1]));                            
                            break;
                        }
                }
            }
            else
                SetNoData(plotTemplate, point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotName"></param>
        /// <param name="plotType"></param>
        /// <param name="data"></param>
        /// <param name="point"></param>
        protected void DrawPlot(PlotItem plotItem)
        {
            PlotTemplate? plotTemplate = null;
            var pointRef = new SKPoint(plotItem.PointRef[0], ImageInfo.Height - plotItem.PointRef[1]);

            if (!string.IsNullOrEmpty(plotItem.Name))
                plotTemplate = pictureTemplate.PlotTemplates.Where(x => x.Name == plotItem.Name)!.FirstOrDefault();

            if (plotTemplate != null)
            {
                // Set Plot Title:
                DrawPlotTitle(plotTemplate, pointRef, (plotItem.IndexRef.Length > 0 ? $" [{plotItem.IndexRef[0]}/{plotItem.IndexRef[1]}]" : string.Empty));

                // Draw layout:
                DrawLayout(plotTemplate, pointRef);

                // Draw array data:
                DrawData(plotTemplate, pointRef, plotItem);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetNoData(PlotTemplate plotTemplate, SKPoint point)
        {
            Surface.Canvas.DrawText("No data", new SKPoint(point.X + plotTemplate.FrameSize[0] / 2 - 20, point.Y - plotTemplate.FrameSize[1] / 2), PaintText);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawPlots()
        {
            foreach (var item in derivedData.PlotItems!)
                DrawPlot(item);
        }

        /// <summary>
        /// 
        /// </summary>
        protected byte[] GeneratePicture()
        {
            DrawPlots();
            //get png from SKSurface
            SKImage img = Surface.Snapshot();
            SKData imgData = img.Encode(SKEncodedImageFormat.Png, 100);
            var image = imgData.ToArray();

            //get binary data
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawPictureTitle()
        {
            Surface.Canvas.DrawText(pictureTemplate.Title, pictureTemplate.PictureDimensions[0] / 2f, pictureTemplate.StartPoint[1] - 20, PaintTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string MakePicture()
        {
            //get png from SKSurface            
            var image = GeneratePicture();
            // Download image:            
            if (pictureTemplate.PicturePreviewFlag)
            {
                string filePath = $"{pictureTemplate.PicturePreviewPath}{Guid}.png";
                File.WriteAllBytes(filePath, image);
            }
            return "data:image/png;base64, " + Convert.ToBase64String(image);
        }
    }
}
