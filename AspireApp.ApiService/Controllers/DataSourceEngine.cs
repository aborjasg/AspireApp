using AspireApp.Libraries;
using AspireApp.ServiceDefaults.Models;
using AspireApp.ServiceDefaults.Shared;
using Newtonsoft.Json;
using System;

namespace AspireApp.ApiService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DataSourceEngine
    {
        protected PictureTemplate template = new ();
        protected DerivedData derivedData;
        protected const string templatesPath = "PictureTemplates.json";

        public DataSourceEngine(string name) 
        {
            derivedData = new DerivedData() { Name= name };
            using (StreamReader r = new StreamReader(templatesPath))
            {
                string json = r.ReadToEnd();
                var templates = JsonConvert.DeserializeObject<PictureTemplate[]>(json);
                if (templates != null)
                {
                    template = templates!.Where(x => x.Name == derivedData.Name)!.FirstOrDefault()!;                       
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private List<PlotItem> GetDataSource(string testType, PictureTemplate template)
        {
            var result = new List<PlotItem>();
            switch (testType)
            {
                case "Combined_NCP":
                    {
                        var random = new Random();
                        // Preparing data sample:
                        for (int j = 0; j < template.PictureLayout[1]; j++)
                            for (int i = 0; i < template.PictureLayout[0]; i++)
                            {
                                var arrData = new double[Constants.NUM_COLS, Constants.NUM_ROWS];
                                for (int k = 0; k < 100; k++)
                                    arrData[random.Next(0, Constants.NUM_COLS), random.Next(0, Constants.NUM_ROWS)] = 1;
                                arrData[0, 1] = 1;
                                if (i == 0 && j == 0) // For testing purposes
                                    arrData = null;
                                
                                result.Add(new PlotItem() { Name = testType, ArrayData = arrData!, PointRef = [template.StartPoint[0] + (i * (288 + template.PlotSpacing[0])), template.StartPoint[1] + (j * (192 + template.PlotSpacing[1]))], IndexRef = [ i, j ] });
                            }
                        break;
                    }
                case "LineChart":
                    {                        
                        var arrData = Calculations.GetChartData();
                        result.Add(new PlotItem() { Name = testType, ArrayData = arrData, PointRef = template.StartPoint });
                        break;
                    }
            }
            return result;
        }

        public DerivedData GetDerivedData() 
        {
            if (template != null)
                derivedData.PlotItems = GetDataSource(derivedData.Name, template);
            return derivedData; 
        }
        public PictureTemplate GetPictureTemplate() { return template; }
    }
}
