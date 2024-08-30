using AspireApp.ApiService.Controllers;
using AspireApp.ApiService.DataAccess;
using AspireApp.ServiceDefaults.Models;
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
        var engine = new DataSourceEngine(derivedData.Name);
        var template = engine.GetPictureTemplate();
        var plotter = new PlotBase(new PictureEngine(template, derivedData));
        var image = plotter.MakePicture()!;
        var metadata = new RunMetadata(derivedData.Name, template);
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
