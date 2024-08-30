using SkiaSharp;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace AspireApp.ApiService.Controllers
{
    public class PlotBase
    {
        protected PictureEngine PictureEngine;

        /// <summary>
        /// 
        /// </summary>
        public PlotBase(PictureEngine pictureEngine)
        {
            PictureEngine = pictureEngine;
        }

        #region Protected/Private Methods

        #endregion

        #region Public Methods

        public string MakePicture()
        {
            return PictureEngine.MakePicture();
        }

        #endregion
    }
}
