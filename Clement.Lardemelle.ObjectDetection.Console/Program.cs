using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Clement.Lardemelle.ObjectDetection;

namespace Clement.Lardemelle.ObjectDetection.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please provide the path to the directory containing the scene images.");
                return;
            }
            
            var sceneDirectory = args[0];
            if (!Directory.Exists(sceneDirectory))
            {
                System.Console.WriteLine($"The directory '{sceneDirectory}' does not exist.");
                return;
            }
            
            var imagePaths = Directory.EnumerateFiles(sceneDirectory, "*.jpg");
            var imageBytesList = new List<byte[]>();

            foreach (var imagePath in imagePaths)
            {
                var imageBytes = await File.ReadAllBytesAsync(imagePath);
                imageBytesList.Add(imageBytes);
            }
            var objectDetection = new ObjectDetection();
            var detectObjectInScenesResults = await objectDetection.DetectObjectInScenesAsync(imageBytesList);
            foreach (var objectDetectionResult in detectObjectInScenesResults)
            {
                System.Console.WriteLine($"Box:{JsonSerializer.Serialize(objectDetectionResult.Box)}");
            }
        }
    }
}