using System;
using Azure.Storage.Queues.Models;
using Azure_Devops_assignment.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azure_Devops_assignment.Service.Interface;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic;

namespace Azure_Devops_assignment.Triggers
{
    public class JobRequest_QueueTrigger
    {
        private readonly ILogger _logger;
        private readonly IWeatherDataService _WeatherDataService;
        private readonly IJobStatusService _JobStatusService;

        public JobRequest_QueueTrigger(ILoggerFactory loggerFactory, IWeatherDataService weatherDataService, IJobStatusService jobStatusService)
        {
            _logger = loggerFactory.CreateLogger<JobRequest_QueueTrigger>();
            _WeatherDataService = weatherDataService;
            _JobStatusService = jobStatusService;
        }

        [Function(nameof(JobRequest_QueueTrigger))]
        [QueueOutput("ImageGenerationJob")]
        public async Task<List<WeatherDataJob>> RunAsync([QueueTrigger("JobRequest", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            try 
            {
                _logger.LogInformation($"C# Queue trigger function processed a job request");

                // Deserialize the message into a JobRequest instance
                JobRequest? jobRequest = JsonSerializer.Deserialize<JobRequest>(message.MessageText);

                if (jobRequest == null)
                {
                    _logger.LogCritical("Message was empty");
                    return null;
                }

                _logger.LogInformation("Updating job status for: " + jobRequest.JobRequestId);
                JobStatusEntity jobStatusEntity = await _JobStatusService.GetJobStatusAsync(jobRequest.Timestamp, jobRequest.JobRequestId);
                jobStatusEntity.Status = Model.Enum.JobStatus.InProgress.ToString();
                await _JobStatusService.UpdateJobStatusAsync(jobStatusEntity);

                _logger.LogInformation($"Deserialized JobRequestId: {jobRequest.JobRequestId}");
                return _WeatherDataService.GetWeatherDataJobs(jobRequest.JobRequestId, jobRequest.Timestamp);
            } 
            catch (Exception ex) 
            {
                _logger.LogError("Something went wrong. Error: " + ex.Message);
                return null;
            }
        }
    }
}
