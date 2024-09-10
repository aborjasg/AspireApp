using AspireApp.ApiService.DataAccess;
using AspireApp.Libraries;
using AspireApp.Libraries.Enums;
using AspireApp.Libraries.Models;
using AspireApp.Libraries.PictureMaker;
using AspireApp.ServiceDefaults.Shared;
using Grpc.Core;
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

app.MapPost("/getSourceData",  (ConnectionString connectionString, DerivedDataFilter filter) =>
{    
    var engine = new DataSourceEngine(filter.Name);
    var result = engine.GetDerivedData();
    return new ActionResponse() { Type = "Information", Message = "OK", Content = UtilsForMessages.Compress(UtilsForMessages.SerializeObject(result)) };
});

app.MapPost("/processData", (ConnectionString connectionString, RunImage record) =>
{
    if (!string.IsNullOrEmpty(record.DataSource))
    {
        var derivedData = UtilsForMessages.DeserializeObject<DerivedData>(UtilsForMessages.Decompress(record.DataSource))!;
        var dataEngine = new DataSourceEngine(derivedData.Name);
        var pictureTemplate = dataEngine.GetPictureTemplate();
        IPlotEngine? plotEngine = Enum.Parse<enmTestType>(derivedData.Name) switch
        {
            enmTestType.ncps => new PlotterNCP(),
            enmTestType.spectrum => new PlotterSpectrum(),
            enmTestType.energy => new PlotterEnergy(),
            enmTestType.uniformity => new PlotterUniformity(),
            _ => null
        };
        var pictureEngine = new PictureEngine(pictureTemplate, derivedData, plotEngine!);
        var image = pictureEngine.MakePicture()!;
        var metadata = new RunMetadata(derivedData.Name, pictureTemplate);
        return new RunImage(derivedData.Name, metadata, derivedData, image);        
    }
    else
        return new RunImage();
});

app.MapPost("/saveRunImage", (ConnectionString connectionString, RunImage record) =>
{
    var obj = new TableAccess<RunImage>(connectionString.FATCloud_Visualization);                
    return new ActionResponse() { Type = "OK", Id = obj.SaveRow(record) }; 
});

app.MapGet("/getRunImage/{id}", (ConnectionString connectionString, int id) =>
{
    var obj = new TableAccess<RunImage>(connectionString.FATCloud_Visualization);
    return obj.GetRow(id);
});

app.MapDefaultEndpoints();

app.Run();
