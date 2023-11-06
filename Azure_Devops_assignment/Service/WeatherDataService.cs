using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Azure_Devops_assignment.Service
{
    public class WeatherDataService : IWeatherDataService
    {
        private readonly string? _ApiUrl;
        private readonly ILogger _logger;

        public WeatherDataService(ILoggerFactory loggerFactory)
        {
            _ApiUrl = Environment.GetEnvironmentVariable("BuienraderAPI");
            _logger = loggerFactory.CreateLogger<WeatherDataService>();
        }

        public List<WeatherDataJob> GetWeatherDataJobs(string jobid, string timestamp)
        {
            try
            {
                List<StationMeasurement> stationMeasurements = GetWeatherDataFromApiAsync().Result;

                List<WeatherDataJob> weatherDataJobs = new List<WeatherDataJob>();

                for (int i = 0; i < stationMeasurements.Count; i++)
                {
                    bool isLast = (i == stationMeasurements.Count - 1); // Check if it's the last item
                    WeatherDataJob weatherDataJob = new WeatherDataJob(jobid, timestamp, stationMeasurements[i], isLast);
                    weatherDataJobs.Add(weatherDataJob);
                }

                _logger.LogInformation("Successfully got data from API");
                return weatherDataJobs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WeatherDataJobs");
                throw;
            }
        }


        private async Task<List<StationMeasurement>> GetWeatherDataFromApiAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(_ApiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonData = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(jsonData);

                    if (jsonObject["actual"]["stationmeasurements"] == null)
                    {
                        throw new Exception("Failed to get data from Api");
                    }

                    JToken? stationMeasurementsToken = jsonObject["actual"]["stationmeasurements"];

                    List<StationMeasurement> stationMeasurements = stationMeasurementsToken.ToObject<List<StationMeasurement>>();

                    return stationMeasurements;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
