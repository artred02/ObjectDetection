using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime.Tensors;
using ObjectDetection;

namespace Clement.Lardemelle.ObjectDetection
{
    public class ObjectDetection
    {
        private readonly Yolo _tinyYolo;

        public ObjectDetection()
        {
            // Initialisation de TinyYoloModel (à adapter en fonction du constructeur)
            _tinyYolo = new Yolo();
        }

        public async Task<List<ObjectDetectionResult>> DetectObjectInScenesAsync(IEnumerable<byte[]> imagePaths)
        {
            await Task.Delay(1000);
            var tinyYolo = new Yolo();
            var listResult = new List<ObjectDetectionResult>();
            foreach (var path in imagePaths)
            {
                var detectedObjects = tinyYolo.Detect(path);
                listResult.Add(new ObjectDetectionResult
                {
                    ImageData = detectedObjects.ImageData,
                    Box = detectedObjects.Boxes
                });
            }
            return listResult;
        }

        private ObjectDetectionResult ProcessImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"L'image n'a pas été trouvée : {imagePath}");

            var imageData = File.ReadAllBytes(imagePath);

            var detectedObjects = _tinyYolo.Detect(imageData);

            return new ObjectDetectionResult
            {
                ImageData = detectedObjects.ImageData,
                Box = detectedObjects.Boxes
            };
        }
    }
}
