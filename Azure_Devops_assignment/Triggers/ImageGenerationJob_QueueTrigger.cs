using System;
using Azure.Storage.Queues.Models;
using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure_Devops_assignment.Triggers
{
    public class ImageGenerationJob_QueueTrigger
    {
        private readonly ILogger _logger;
        private readonly IImageService _imageService;
        private readonly IAzureBlobService _azureBlobService;

        public ImageGenerationJob_QueueTrigger(ILoggerFactory loggerFactory, IImageService imageService, IAzureBlobService azureBlobService)
        {
            _logger = loggerFactory.CreateLogger<ImageGenerationJob_QueueTrigger>();
            _imageService = imageService;
            _azureBlobService = azureBlobService;
        }

        [Function(nameof(ImageGenerationJob_QueueTrigger))]
        public async Task RunAsync([QueueTrigger("ImageGenerationJob", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            try
            {
                _logger.LogInformation($"ImageGenerationJob_QueueTrigger function processed.");
                WeatherDataJob? data = JsonConvert.DeserializeObject<WeatherDataJob>(message.MessageText);

                if (data == null)
                {
                    return;
                }

                byte[] image = _imageService.GetImageForStationMeasurementAsync(data.Measurement).Result;
                await _azureBlobService.SetBlobContainerClientOrCreateAsync(data.JobId);
                await _azureBlobService.UploadBlobAsync(image, data.Measurement.stationname.Replace(" ", "_") + ".png");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong. Error: " + ex.Message);
                return;
            }

        }
    }
}
