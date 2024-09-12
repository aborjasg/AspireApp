using AspireApp.Libraries;
using AspireApp.Libraries.Enums;
using AspireApp.Libraries.Models;
using AspireApp.ServiceDefaults.Shared;
using Newtonsoft.Json;
using System;

namespace AspireApp.Libraries.PictureMaker
{
    /// <summary>
    /// 
    /// </summary>
    public class DataSourceEngine
    {
        protected PictureTemplate template = new();
        protected DerivedData derivedData;
        protected const string templatesPath = "PictureTemplates.json";

        public DataSourceEngine(string name, enmTestType testType)
        {
            derivedData = new DerivedData() { Name = name, TestType = testType };
            using (StreamReader r = new StreamReader(templatesPath))
            {
                string json = r.ReadToEnd();
                var templates = JsonConvert.DeserializeObject<PictureTemplate[]>(json);
                if (templates != null)
                {
                    template = templates!.Where(x => x.TestType == derivedData.TestType)!.FirstOrDefault()!;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private List<PlotItem> GetDataSource(enmTestType testType, PictureTemplate template)
        {
            var result = new List<PlotItem>();
            switch (testType)
            {
                case enmTestType.ncps:
                    {
                        var random = new Random();
                        // Preparing data sample:
                        for (int j = 0; j < template.PictureLayout[1]; j++)
                            for (int i = 0; i < template.PictureLayout[0]; i++)
                            {
                                double[,]? arrData;
                                if (i == 0 && j == 0) // For testing purposes
                                    arrData = null;
                                else
                                    arrData = FakeData.GetNcpData();

                                result.Add(new PlotItem() { Name = "Combined NCPs", PlotType = enmPlotType.ncp, ArrayData = arrData!, PointRef = [template.StartPoint[0] + i * (288 + template.PlotSpacing[0]), template.StartPoint[1] + j * (192 + template.PlotSpacing[1])], IndexRef = [i, j] });
                            }
                        break;
                    }
                case enmTestType.spectrum:
                    {
                        var arrData1 = FakeData.GetLineChartData();
                        var arrData2 = FakeData.GetHistogramData();
                        result.Add(new PlotItem() { Name = "Spectrum [Linechart]", PlotType = enmPlotType.linechart, ArrayData = arrData1, PointRef = template.StartPoint, IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "Spectrum [Histogram]", PlotType = enmPlotType.histogram1, ArrayData = arrData2, PointRef = [template.StartPoint[0] + template.PlotSpacing[0] + template.PictureDimensions[0] / 2, template.StartPoint[1]], IndexRef = [0, 1] });
                        break;
                    }
                case enmTestType.energy: case enmTestType.electrical:
                    {
                        var arrData = FakeData.GetHeatMapData();
                        result.Add(new PlotItem() { Name = "Electrical [Heatmap]", PlotType = enmPlotType.heatmap, ArrayData = arrData, PointRef = template.StartPoint, IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "Electrical [Histogram]", PlotType = enmPlotType.histogram2, ArrayData = arrData, PointRef = [template.StartPoint[0] + template.PlotSpacing[0] + template.PictureDimensions[0] / 2, template.StartPoint[1]], IndexRef = [0, 1] });
                        break;
                    }
                case enmTestType.energy_cal:
                    {
                        result.Add(new PlotItem() { Name = "XRAY-RAW-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayRaw(), PointRef = template.StartPoint, IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "XRAY-PB-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayPB(), PointRef = [template.StartPoint[0] + template.PictureDimensions[0] / 3, template.StartPoint[1]], IndexRef = [0, 1] });
                        result.Add(new PlotItem() { Name = "XRAY-CEO2-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayCEO2(), PointRef = [template.StartPoint[0] + ( template.PictureDimensions[0] / 3) * 2, template.StartPoint[1]], IndexRef = [0, 2] });
                        break;
                    }
            }
            return result;
        }

        public DerivedData GetDerivedData()
        {
            if (template != null)
                derivedData.PlotItems = GetDataSource(derivedData.TestType, template);
            return derivedData;
        }
        public PictureTemplate GetPictureTemplate() { return template; }
    }
}
