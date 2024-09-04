using AspireApp.Libraries;
using AspireApp.ServiceDefaults.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspireApp.ApiService.Controllers
{
    public class PictureEngine
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
        /// <summary>
        /// 
        /// </summary>
        public SKColor[]? Brushes { get; set; }

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
        /// <param name="plotImage"></param>
        protected void SetUpLayout(PlotTemplate plotImage)
        {
            // Set Frame Size:
            if (plotImage.FrameSize.Length > 0)
            {
                double rangeX = 0, rangeY = 0, width = 0, height = 0;

                switch (plotImage.PlotType)
                {                    
                    case enmPlotType.ncp:
                        {
                            // Get Scales:
                            plotImage.FrameSize = new float[] { Constants.NUM_COLS * plotImage.StrokeWidth, Constants.NUM_ROWS * plotImage.StrokeWidth };
                            PaintPoint.StrokeWidth = plotImage.StrokeWidth;             
                            
                            width = plotImage.FrameSize[0] - plotImage.Axis[0].Offset[0] - plotImage.Axis[0].Offset[1];
                            height = plotImage.FrameSize[1] - plotImage.Axis[1].Offset[0] - plotImage.Axis[1].Offset[1];
                            
                            AxisValues = new double[][] { plotImage.Axis[0].Range, plotImage.Axis[1].Range };
                            rangeX = AxisValues[0][1] - AxisValues[0][0];
                            rangeY = AxisValues[1][1] - AxisValues[1][0];
                            
                            break;
                        }
                    case enmPlotType.linechart:
                        {
                            var range = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);
                            
                            plotImage.Axis[0].Range = new double[] { range[0], range[range.Length - 1], 1 };
                            range = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                            plotImage.Axis[1].Range = new double[] { range[0], range[range.Length - 1], 1 };

                            AxisValues = new double[][] { plotImage.Axis[0].Range, plotImage.Axis[1].Range };
                            rangeX = AxisValues[0][1] - AxisValues[0][0];
                            rangeY = AxisValues[1][1] - AxisValues[1][0];

                            break;
                        }
                }

                if (rangeX != 0)
                    plotImage.Axis[0].Scale = width / rangeX;
                else
                    plotImage.Axis[0].Scale = 1;

                if (rangeY != 0)
                    plotImage.Axis[1].Scale = height / rangeY;
                else
                    plotImage.Axis[1].Scale = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="point"></param>
        /// <param name="PlotTemplate"></param>
        protected void DrawLayout(PlotTemplate plotImage, SKPoint point)
        {
            SetUpLayout(plotImage);

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

            switch (plotImage.AreaLayout)
            {
                case enmAreaLayout.Plain:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotImage.FrameSize[0], y0 = point.Y, y1 = point.Y - plotImage.FrameSize[1];
                                                                       
                        Surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x0, y1), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x0, y0), new SKPoint(x1, y0), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x0, y1), new SKPoint(x1, y1), paintSquare);
                        Surface.Canvas.DrawLine(new SKPoint(x1, y0), new SKPoint(x1, y1), paintSquare);

                        break;
                    }
                case enmAreaLayout.Squad:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotImage.FrameSize[0], y0 = point.Y, y1 = point.Y + plotImage.FrameSize[1];
                        float space = plotImage.StrokeWidth;

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
            DrawPlotTitle(plotImage, point, $" [{plotImage.Index[0]}/{plotImage.Index[1]}]");
            DrawAxis(plotImage, point);
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

                for (double k = AxisValues[0][0]; k < AxisValues[0][1]; k += AxisValues[0][2])
                {
                    var (x, y) = ((float)(point.X + plotTemplate.Axis[0].Offset[0] + (k - AxisValues[0][0]) * plotTemplate.Axis[0].Scale), point.Y);
                    var pointRef = new SKPoint(x, y);

                    Surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X, pointRef.Y + 5), PaintBorder);
                    Surface.Canvas.DrawText(k.ToString(numDecimals), new SKPoint(pointRef.X, pointRef.Y + 17), paint);
                }

                // Y Axis:
                paint.TextAlign = SKTextAlign.Right;

                for (double k = AxisValues[1][0]; k < AxisValues[1][1]; k += AxisValues[1][2])
                {
                    var (x, y) = (point.X, (float)(point.Y + plotTemplate.Axis[1].Offset[0] + (k - AxisValues[1][0]) * plotTemplate.Axis[1].Scale));
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
                                        Surface.Canvas.DrawPoint(new SKPoint(point.X + px, point.Y + py), PaintPoint);
                                    }
                                }
                            }
                            break;
                        }
                    case enmPlotType.linechart:
                        {
                            var arrX = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(0), null }!);                            
                            var arrY = (double[])derivedData.PlotItems![0].ArrayData!.PartOf(new SliceIndex?[] { new SliceIndex(1), null }!);
                            
                            AxisValues = new double[][] { Array.Empty<double>(), Array.Empty<double>() };

                            // X axis:
                            int totalMaxJumps = 10;
                            var (minX, maxX, baseX) = DataTransformation.AdjustLimits(0, arrX.Length, totalMaxJumps, true);
                            AxisValues[0] = new double[] { minX, maxX, baseX };

                            // Y axis:
                            totalMaxJumps = 6;
                            var (minY, maxY, baseY) = DataTransformation.AdjustLimits(arrY.Min(), arrY.Max(), totalMaxJumps, true);
                            AxisValues[1] = new double[] { minY, maxY, baseY };

                            var path = new SKPath();                            
                            float px, py;

                            path = new SKPath();
                            path.MoveTo(point);

                            for (int i = 0; i < arrX.Length; i++)
                            {
                                px = (float)(point.X + arrX[i] * plotTemplate.Axis[0].Scale);
                                py = (float)(point.Y - arrY[i] * plotTemplate.Axis[1].Scale);

                                if (i == 0)
                                    path.MoveTo(px, py);
                                else
                                    path.LineTo(px, py);
                            }

                            // Draw into canvas:
                            var paint = new SKPaint()
                            {
                                Color = new SKColor(31, 119, 180),
                                IsAntialias = false,
                                IsStroke = false,
                                StrokeWidth = 2f,
                                Style = SKPaintStyle.Stroke
                            };

                            Surface.Canvas.DrawPath(path, paint);

                            path.Close();
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
            PlotTemplate? plotImage = null;
            var pointRef = new SKPoint(plotItem.PointRef[0], ImageInfo.Height - plotItem.PointRef[1]);

            if (!string.IsNullOrEmpty(plotItem.Name))
                plotImage = pictureTemplate.PlotTemplates.Where(x => x.Name == plotItem.Name)!.FirstOrDefault();

            if (plotImage != null)
            {  
                // Draw layout:
                DrawLayout(plotImage, pointRef);

                // Draw array data:
                DrawData(plotImage, pointRef, plotItem);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetNoData(PlotTemplate plotImage, SKPoint point)
        {
            Surface.Canvas.DrawText("No data", new SKPoint(point.X + plotImage.FrameSize[0] / 2 - 20, point.Y + plotImage.FrameSize[1] / 2), PaintText);
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
