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

        public GetWeatherData_HttpTrigger(ILoggerFactory loggerFactory, IAzureBlobService azureBlobService)
        {
            _logger = loggerFactory.CreateLogger<GetWeatherData_HttpTrigger>();
            _azureBlobService = azureBlobService;
        }

        [Function("GetWeatherData_HttpTrigger")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetWeatherData/{jobId}")] HttpRequestData req,
            string jobId)
        {
            try
            {
                _logger.LogInformation($"C# HTTP trigger function processed a get weather data request for JobId: {jobId}");
                await _azureBlobService.SetBlobContainerClientAsync(jobId);
                Uri uri = _azureBlobService.GetBlobContainerUrlWithSasToken();
                List<string> fileNames = await _azureBlobService.GetBlobNamesAsync();

                BlobContainerSASTokenResponce blobContainerSASTokenResponce = new BlobContainerSASTokenResponce(uri.ToString(), fileNames);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonSerializer.Serialize(blobContainerSASTokenResponce));
                return response;
            }
            catch (Exception)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("Sometin`g went wrong or the token was not valid");
                return response;
            }
        }
    }
}
