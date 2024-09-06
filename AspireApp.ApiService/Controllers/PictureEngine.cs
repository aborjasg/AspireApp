﻿using AspireApp.Libraries;
using AspireApp.Libraries.Models;
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
    public class PictureEngine : IPictureEngine
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
        protected IPlotEngine plotEngine;
        /// <summary>
        /// 
        /// </summary>
        protected string[]? pictureLegend;

        #endregion

        #region Plot Objects

        /// <summary>
        /// 
        /// </summary>
        protected SKImageInfo ImageInfo;
        /// <summary>
        /// 
        /// </summary>
        protected SKSize PictureSize;
        /// <summary>
        /// 
        /// </summary>
        public SKSurface Surface;
        
        #endregion

        public PictureEngine(PictureTemplate pictureTemplate, DerivedData derivedData, IPlotEngine plotEngine)
        {
            Guid = Guid.NewGuid();
            this.pictureTemplate = pictureTemplate;
            ImageInfo = new SKImageInfo(pictureTemplate.PictureDimensions[0], pictureTemplate.PictureDimensions[1]);
            Surface = SKSurface.Create(ImageInfo);
            Surface.Canvas.Clear(SKColors.White);            
            this.derivedData = derivedData;
            this.plotEngine = plotEngine;
        }

        #region Protected/Private Methods

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
                plotTemplate = pictureTemplate.PlotTemplates.Where(x => x.PlotType.ToString() == plotItem.Name)!.FirstOrDefault();

            if (plotTemplate != null)
            {
                // Draw layout:
                plotEngine.SetUpLayout(plotTemplate, plotItem);
                plotEngine.DrawLayout(plotTemplate, pointRef, Surface);

                // Set Plot Title:
                plotEngine.DrawPlotTitle(plotTemplate, pointRef, Surface, (plotItem.IndexRef.Length > 0 ? $" [{plotItem.IndexRef[0]}/{plotItem.IndexRef[1]}]" : string.Empty));

                // Draw array data:
                plotEngine.DrawData(plotTemplate, pointRef, Surface, plotItem);                
            }
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

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void DrawPictureTitle()
        {
            Surface.Canvas.DrawText(pictureTemplate.Title, pictureTemplate.PictureDimensions[0] / 2f, pictureTemplate.StartPoint[1] - 20, Constants.PaintTitle);
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

        #endregion
    }
}
