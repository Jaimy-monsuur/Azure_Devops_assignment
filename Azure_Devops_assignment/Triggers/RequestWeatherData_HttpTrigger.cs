using System.Net;
using System.Text.Json;
using Azure_Devops_assignment.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Devops_assignment.Triggers
{
    public class RequestWeatherData_HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly string? _url;
        private readonly IJobStatusService _jobStatusService;

        public RequestWeatherData_HttpTrigger(ILoggerFactory loggerFactory, IJobStatusService jobStatusService)
        {
            _logger = loggerFactory.CreateLogger<RequestWeatherData_HttpTrigger>();
            _url = Environment.GetEnvironmentVariable("GetWeatherDataBaseUrl");
            _jobStatusService = jobStatusService;
        }

        [Function("RequestWeatherData_HttpTrigger")]
        public async Task<QueueAndHttpResponseDataOutput> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a Http request to generate a job request.");
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string stringID = Guid.NewGuid().ToString() + "-" + timestamp;

            JobRequest job = new JobRequest
            {
                JobRequestId = stringID,
                Timestamp = timestamp,
                BaseUrl = _url,
                JobRequestUrl = _url + stringID,
            };

            JobStatusEntity jobStatusEntity = new JobStatusEntity(stringID, timestamp, Model.Enum.JobStatus.Pending);
            await _jobStatusService.InsertJobStatusAsync(jobStatusEntity);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(JsonSerializer.Serialize(job));

            return new QueueAndHttpResponseDataOutput
            {
                JobRequest = job,
                HttpResponse = response,
            };
        }

        public class QueueAndHttpResponseDataOutput
        {
            [QueueOutput("JobRequest")]
            public JobRequest? JobRequest { get; set; }
            public HttpResponseData? HttpResponse { get; set; }
        }
    }
}
