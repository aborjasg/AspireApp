using AspireApp.ServiceDefaults.Models;
using AspireApp.ServiceDefaults.Shared;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;

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
                plotImage.FrameSize = new float[] { Constants.NUM_COLS * plotImage.StrokeWidth, Constants.NUM_ROWS * plotImage.StrokeWidth };
                PaintPoint.StrokeWidth = plotImage.StrokeWidth;

                // Get Scales:
                double width = plotImage.FrameSize[0] - plotImage.Axis[0].Offset[0] - plotImage.Axis[0].Offset[1];
                double height = plotImage.FrameSize[1] - plotImage.Axis[1].Offset[0] - plotImage.Axis[1].Offset[1];

                AxisValues = new double[][] { plotImage.Axis[0].Range, plotImage.Axis[1].Range };
                double rangeX = AxisValues[0][1] - AxisValues[0][0];
                double rangeY = AxisValues[1][1] - AxisValues[1][0];

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
            switch (plotImage.AreaLayout)
            {
                case enmAreaLayout.Plain:
                    {
                        break;
                    }
                case enmAreaLayout.Squad:
                    {
                        float x0 = (float)point.X, x1 = point.X + plotImage.FrameSize[0], y0 = point.Y, y1 = point.Y + plotImage.FrameSize[1];
                        float space = plotImage.StrokeWidth;

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
        protected void DrawPlotTitle(PlotTemplate plotImage, SKPoint point, string addToTitle = "")
        {
            Surface.Canvas.DrawText(plotImage.Title + addToTitle, point.X + plotImage.FrameSize[0] / 2f, point.Y - 5, PaintTitle);
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
        protected void DrawAxis(PlotTemplate plotImage, SKPoint point)
        {
            SKPaint paint = PaintTextSmall.Clone();

            if (AxisValues != null)
            {
                string numDecimals = "0";
                if (AxisValues[0][2] < 1) numDecimals = AxisValues[0][2] <= 0.25 ? "0.00" : "0.0";

                float[,] offset = new float[2, 2];
                offset[0, 0] = offset[0, 1] = offset[1, 0] = offset[1, 1] = plotImage.StrokeWidth / 2;

                // X Axis:
                paint.TextAlign = SKTextAlign.Center;

                for (double k = AxisValues[0][0]; k < AxisValues[0][1]; k += AxisValues[0][2])
                {
                    var (x, y) = ((float)(point.X + offset[0, 0] + (k - AxisValues[0][0]) * plotImage.Axis[0].Scale), point.Y + plotImage.FrameSize[1]);
                    var pointRef = new SKPoint(x, y);

                    Surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X, pointRef.Y + 5), PaintBorder);
                    Surface.Canvas.DrawText(k.ToString(numDecimals), new SKPoint(pointRef.X, pointRef.Y + 17), paint);
                }

                // Y Axis:
                paint.TextAlign = SKTextAlign.Right;

                for (double k = AxisValues[1][0]; k < AxisValues[1][1]; k += AxisValues[1][2])
                {
                    var (x, y) = (point.X, (float)(point.Y + offset[1, 0] + (k - AxisValues[1][0]) * plotImage.Axis[1].Scale));
                    var pointRef = new SKPoint(x, y);

                    Surface.Canvas.DrawLine(pointRef, new SKPoint(pointRef.X - 5, pointRef.Y), PaintBorder);
                    Surface.Canvas.DrawText(k.ToString(), new SKPoint(pointRef.X - 7, pointRef.Y + 3), paint);
                }
            }
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
            var point = new SKPoint(plotItem.PointRef[0], plotItem.PointRef[1]);

            if (!string.IsNullOrEmpty(plotItem.Name))
                plotImage = pictureTemplate.PlotTemplates.Where(x => x.Name == plotItem.Name)!.FirstOrDefault();

            if (plotImage != null)
            {
                // Preparing plot:        
                var color = SKColors.Black;
                PaintPoint.StrokeWidth = plotImage.StrokeWidth;

                // Draw layout:
                DrawLayout(plotImage, point);

                // Draw array data:
                if (plotItem.ArrayData != null)
                {
                    var arrayData = (double[,])plotItem.ArrayData;
                    for (int col = 0; col < arrayData.GetLength(0); col++)
                    {
                        for (int row = 0; row < arrayData.GetLength(1); row++)
                        {
                            if (arrayData[col, row] > 0)
                            {
                                var (px, py) = (col * plotImage.StrokeWidth + plotImage.StrokeWidth / 2, row * plotImage.StrokeWidth + plotImage.StrokeWidth / 2);
                                PaintPoint.Color = color;
                                Surface.Canvas.DrawPoint(new SKPoint(point.X + px, point.Y + py), PaintPoint);
                            }
                        }
                    }
                }
                else
                    SetNoData(plotImage, point);
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
