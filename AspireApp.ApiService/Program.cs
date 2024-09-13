using AspireApp.ApiService.DataAccess;
using AspireApp.Libraries;
using AspireApp.Libraries.Enums;
using AspireApp.Libraries.Models;
using AspireApp.Libraries.PictureMaker;
using AspireApp.ServiceDefaults.Shared;
using Grpc.Core;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Net.Mail;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ConnectionString>(sp =>
{
    return  new ConnectionString() { FATCloud_Visualization = builder.Configuration["ConnectionString:FATCloud_Visualization"]! }; 
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapPost("/getSourceData", (ConnectionString connectionString, DerivedDataFilter filter) =>
{
    var result = new ActionResponse() { Type = "Information", StartDate = DateTime.Now };
    try
    {
        var engine = new DataSourceEngine(filter.Name, Enum.Parse<enmTestType>(filter.TestType));
        var derivedData = engine.GetDerivedData();
        result.Message = "OK";
        result.Content = UtilsForMessages.Compress(UtilsForMessages.SerializeObject(derivedData));
    }
    catch (Exception ex)
    {
        result.Type = "Error";
        result.Message = ex.Message;
    }
    result.EndDate = DateTime.Now;
    return result;
});

app.MapPost("/processData", (ConnectionString connectionString, RunImage record) =>
{
    var result = new ActionResponse() { Type = "Information", StartDate = DateTime.Now };    
    try
    {
        if (!string.IsNullOrEmpty(record.DataSource))
        {
            var derivedData = UtilsForMessages.DeserializeObject<DerivedData>(UtilsForMessages.Decompress(record.DataSource))!;
            var dataEngine = new DataSourceEngine(derivedData.Name, derivedData.TestType);
            var pictureTemplate = dataEngine.GetPictureTemplate();
            IPlotEngine? plotEngine = derivedData.TestType switch
            {
                enmTestType.ncps => new PlotterNCP(),
                enmTestType.spectrum => new PlotterSpectrum(),
                enmTestType.energy => new PlotterEnergy(),
                enmTestType.energy_cal => new PlotterEnergyCal(),
                enmTestType.uniformity => new PlotterUniformity(),
                _ => null
            };
            var pictureEngine = new PictureEngine(pictureTemplate, derivedData, plotEngine!);
            var image = pictureEngine.MakePicture()!;
            var metadata = new RunMetadata(derivedData.Name, pictureTemplate);
            record = new RunImage(derivedData.Name, metadata, derivedData, image);

            result.Message = "OK";
            result.Content = UtilsForMessages.Compress(UtilsForMessages.SerializeObject(record));
        }
        else
            throw new Exception("DataSource invalid");
    }
    catch (Exception ex)
    {
        result.Type = "Error";
        result.Message = ex.Message;
    }
    result.EndDate = DateTime.Now;
    return result;
});

app.MapPost("/saveRunImage", (ConnectionString connectionString, RunImage record) =>
{
    var result = new ActionResponse() { Type = "Information", StartDate = DateTime.Now };
    try
    {
        var obj = new TableAccess<RunImage>(connectionString.FATCloud_Visualization);
        result = new ActionResponse() { Type = "OK", Id = obj.SaveRow(record) };
    }
    catch (Exception ex)
    {
        result.Type = "Error";
        result.Message = ex.Message;
    }
    result.EndDate = DateTime.Now;
    return result;
});

app.MapGet("/getRunImage/{id}", (ConnectionString connectionString, int id) =>
{
    var result = new ActionResponse() { Type = "Information", StartDate = DateTime.Now };
    var record = new RunImage();
    try
    {
        var obj = new TableAccess<RunImage>(connectionString.FATCloud_Visualization);
        record = obj.GetRow(id);

        result.Message = "OK";
        result.Content = UtilsForMessages.Compress(UtilsForMessages.SerializeObject(record));
    }
    catch (Exception ex)
    {
        result.Type = "Error";
        result.Message = ex.Message;
    }
    result.EndDate = DateTime.Now;
    return result;
});

app.MapDefaultEndpoints();

app.Run();
