using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Azure_Devops_assignment.Triggers
{
    public class ImageGenerationJob_QueueTrigger
    {
        private readonly ILogger<ImageGenerationJob_QueueTrigger> _logger;

        public ImageGenerationJob_QueueTrigger(ILogger<ImageGenerationJob_QueueTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ImageGenerationJob_QueueTrigger))]
        public void Run([QueueTrigger("ImageGenerationJob", Connection = "")] QueueMessage message)
        {
            _logger.LogInformation($"ImageGenerationJob_QueueTrigger function processed: {message.MessageText}");
        }
    }
}
