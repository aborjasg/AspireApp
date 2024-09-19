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
        protected PictureTemplate pictureTemplate = new();
        protected DerivedData derivedData;
        protected const string templatesPath = "PictureTemplates.json";

        public DataSourceEngine(string name)
        {
            derivedData = new DerivedData() { Name = name };
            using (StreamReader r = new StreamReader(templatesPath))
            {
                string json = r.ReadToEnd();
                var templates = JsonConvert.DeserializeObject<PictureTemplate[]>(json);
                if (templates != null)
                {
                    pictureTemplate = templates!.Where(x => x.Name == derivedData.Name)!.FirstOrDefault()!;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="pictureTemplate"></param>
        /// <returns></returns>
        private List<PlotItem> GetDataSource(PictureTemplate pictureTemplate)
        {
            var result = new List<PlotItem>();
            switch (pictureTemplate.TestType)
            {
                case enmTestType.ncps:
                    {
                        var random = new Random();
                        // Preparing data sample:
                        for (int j = 0; j < pictureTemplate.PictureLayout[1]; j++)
                            for (int i = 0; i < pictureTemplate.PictureLayout[0]; i++)                            
                                result.Add(new PlotItem() { Name = "Combined NCPs", PlotType = enmPlotType.ncp, ArrayData = FakeData.GetNcpData(), IndexRef = [i, j] });
                            
                        break;
                    }
                case enmTestType.heatmapDM:
                    {
                        var random = new Random();
                        // Preparing data sample:
                        for (int j = 0; j < pictureTemplate.PictureLayout[1]; j++)
                            for (int i = 0; i < pictureTemplate.PictureLayout[0]; i++)
                                result.Add(new PlotItem() { Name = "Heatmap DM", PlotType = enmPlotType.heatmap, ArrayData = FakeData.GetHeatMapData(), IndexRef = [i, j] });

                        break;
                    }
                case enmTestType.spectrum:
                    {
                        result.Add(new PlotItem() { Name = "Spectrum [Linechart]", PlotType = enmPlotType.linechart, ArrayData = FakeData.GetLineChartData(), IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "Spectrum [Histogram]", PlotType = enmPlotType.histogram1, ArrayData = FakeData.GetHistogramData(), IndexRef = [1, 0] });
                        break;
                    }
                case enmTestType.energy: case enmTestType.electrical:
                    {
                        var arrData = FakeData.GetHeatMapData();
                        result.Add(new PlotItem() { Name = "Electrical [Heatmap]", PlotType = enmPlotType.heatmap, ArrayData = arrData, IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "Electrical [Histogram]", PlotType = enmPlotType.histogram2, ArrayData = arrData, IndexRef = [1, 0] });
                        break;
                    }
                case enmTestType.energy_cal:
                    {
                        result.Add(new PlotItem() { Name = "XRAY-RAW-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayRaw(), IndexRef = [0, 0] });
                        result.Add(new PlotItem() { Name = "XRAY-PB-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayPB(), IndexRef = [1, 0] });
                        result.Add(new PlotItem() { Name = "XRAY-CEO2-K-EDGE_1000_ENERGY_CALIBRATION [spec_mean]", PlotType = enmPlotType.curvechart, ArrayData = FakeData.GetEnergyCal_XrayCEO2(), IndexRef = [2, 0] });
                        break;
                    }
                case enmTestType.stability:
                    {
                        var arrData1 = (Array)FakeData.GetStability_DNumber();
                        var arrData2 = (Array)FakeData.GetStability_DNumber_Ncp();
                        var data1 = (double[,])arrData1.PartOf(new SliceIndex?[] { new SliceIndex(0), null, null }!);
                        var data2 = (double[,])arrData2.PartOf(new SliceIndex?[] { new SliceIndex(0), null, null }!);

                        result.Add(new PlotItem() { Name = "D-Number [Heatmap]", PlotType = enmPlotType.heatmap_stability, ArrayData = data1, IndexRef = [0, 2] });
                        result.Add(new PlotItem() { Name = "D-Number [Histogram]", PlotType = enmPlotType.histogram_stability, ArrayData = data1, IndexRef = [0, 1] });
                        result.Add(new PlotItem() { Name = "D-Number [NCP]", PlotType = enmPlotType.ncp, ArrayData = data2, IndexRef = [0, 0] });
                        break;
                    }
            }
            return result;
        }

        public DerivedData GetDerivedData()
        {
            if (pictureTemplate != null)
                derivedData.PlotItems = GetDataSource(pictureTemplate);
            return derivedData;
        }
        public PictureTemplate GetPictureTemplate() { return pictureTemplate; }
    }
}
