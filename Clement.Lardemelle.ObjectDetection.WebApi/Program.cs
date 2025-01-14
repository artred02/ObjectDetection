using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/ObjectDetection", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count < 1)
        return Results.BadRequest();
    using var sceneSourceStream = files[0].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();
    
    var objectDetection = new Clement.Lardemelle.ObjectDetection.ObjectDetection();
    var detectObjectInScenesResults = await objectDetection.DetectObjectInScenesAsync(new List<byte[]> {imageSceneData});
    var imageData = new byte[]{};
    foreach (var objectDetectionResult in detectObjectInScenesResults)
    {
        imageData = objectDetectionResult.ImageData;
    }
    
    return Results.File(imageData, "image/jpg");
}).DisableAntiforgery();

app.Run();