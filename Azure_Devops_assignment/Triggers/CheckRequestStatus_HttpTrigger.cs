using System.Net;
using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Devops_assignment.Triggers
{
    public class CheckRequestStatus_HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly IJobStatusService _jobStatusService;

        public CheckRequestStatus_HttpTrigger(ILoggerFactory loggerFactory, IJobStatusService jobStatusService)
        {
            _logger = loggerFactory.CreateLogger<CheckRequestStatus_HttpTrigger>();
            _jobStatusService = jobStatusService;
        }

        [Function("CheckRequestStatus_HttpTrigger")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "CheckRequestStatus_HttpTrigger/{jobId}")] HttpRequestData req,
            string jobId)
        {
            try
            {
                _logger.LogInformation($"C# HTTP trigger function processed a status check request for JobId: {jobId}");
                List<string> parts = jobId.Split('-').ToList();
                _logger.LogInformation("Getting status for id " + jobId + " Timestamp: " + parts.Last());
                JobStatusEntity jobStatus = await _jobStatusService.GetJobStatusAsync(parts.Last(), jobId);
                if (jobStatus == null)
                {
                    throw new Exception("Job status is not present for id: " + jobId);
                }

                return CreateResponse(req, HttpStatusCode.OK, $"Job Status: {jobStatus.Status}");
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
