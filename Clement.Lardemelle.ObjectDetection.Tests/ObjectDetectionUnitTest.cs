using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


namespace Clement.Lardemelle.ObjectDetection.Tests;

public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }
        var detectObjectInScenesResults = await new
            ObjectDetection().DetectObjectInScenesAsync(imageScenesData);
        Assert.Equal(
            "[{\"Dimensions\":{\"X\":316.49274,\"Y\":166.0512,\"Height\":72.08433,\"Width\":85.20762},\"Label\":\"car\",\"Confidence\":0.70770496},{\"Dimensions\":{\"X\":255.23544,\"Y\":178.30383,\"Height\":29.065924,\"Width\":46.222485},\"Label\":\"car\",\"Confidence\":0.52503866}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Box)
        );

        Assert.Equal(
            "[{\"Dimensions\":{\"X\":106.86213,\"Y\":94.00874,\"Height\":147.19458,\"Width\":205.93983},\"Label\":\"car\",\"Confidence\":0.33103877}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[1].Box)
        );
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        // log result
        return executingPath;
    }
}