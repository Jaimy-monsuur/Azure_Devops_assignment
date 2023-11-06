using System.Net;
using System.Text.Json;
using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Devops_assignment.Triggers
{
    public class GetWeatherData_HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IJobStatusService _jobStatusService;

        public GetWeatherData_HttpTrigger(ILoggerFactory loggerFactory, IAzureBlobService azureBlobService, IJobStatusService jobStatusService)
        {
            _logger = loggerFactory.CreateLogger<GetWeatherData_HttpTrigger>();
            _azureBlobService = azureBlobService;
            _jobStatusService = jobStatusService;
        }

        [Function("GetWeatherData_HttpTrigger")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetWeatherData_HttpTrigger/{jobId}")] HttpRequestData req,
            string jobId)
        {
            try
            {
                _logger.LogInformation($"C# HTTP trigger function processed a get weather data request for JobId: {jobId}");
                List<string> parts = jobId.Split('-').ToList();
                _logger.LogInformation("Getting status for id" + jobId + "Timestamp: " + parts.Last());
                JobStatusEntity jobStatus = await _jobStatusService.GetJobStatusAsync(parts.Last(), jobId);
                if (jobStatus == null)
                {
                    throw new Exception("Job status is not present for id: " + jobId);
                }
                if (jobStatus.Status != Model.Enum.JobStatus.Completed.ToString())
                {
                    return CreateResponse(req, HttpStatusCode.OK, "Request is still being processed, Status: " + jobStatus.Status);
                }

                await _azureBlobService.SetBlobContainerClientAsync(jobId);
                Uri uri = _azureBlobService.GetBlobContainerUrlWithSasToken();
                List<string> fileNames = await _azureBlobService.GetBlobNamesAsync();

                BlobContainerSASTokenResponce blobContainerSASTokenResponce = new BlobContainerSASTokenResponce(uri.ToString(), fileNames);
                return CreateResponse(req, HttpStatusCode.OK, JsonSerializer.Serialize(blobContainerSASTokenResponce));
            }
            catch (Exception)
            {
                return CreateResponse(req, HttpStatusCode.InternalServerError, "Something went wrong or the token was not valid");
            }
        }

        private HttpResponseData CreateResponse(HttpRequestData req, HttpStatusCode statusCode, string content)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(content);
            return response;
        }
    }
}
