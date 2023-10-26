using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Azure_Devops_assignment.Service
{
    public class WeatherDataService : IWeatherDataService
    {
        private readonly string? _ApiUrl;
        private readonly ILogger _logger;

        public WeatherDataService(ILoggerFactory loggerFactory)
        {
            _ApiUrl = Environment.GetEnvironmentVariable("ImageApi");
            _logger = loggerFactory.CreateLogger<WeatherDataService>();
        }


        public List<WeatherDataJob> GetWeatherDataJobs(string jobid, string timestamp)
        {
            try
            {
                List<StationMeasurement> stationMeasurements = GetWeatherDataFromApiAsync().Result;

                List<WeatherDataJob> weatherDataJobs = new List<WeatherDataJob>();

                foreach (var stationMeasurement in stationMeasurements)
                {
                    WeatherDataJob weatherDataJob = new WeatherDataJob(jobid, timestamp, stationMeasurement);
                    weatherDataJobs.Add(weatherDataJob);
                }

                return weatherDataJobs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WeatherDataJobs");
                throw;
            }
        }

        public async Task<List<StationMeasurement>> GetWeatherDataFromApiAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(_ApiUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonData = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonData);
                    JToken? stationMeasurementsToken = jsonObject["actual"]["stationmeasurements"];
                    List<StationMeasurement> stationMeasurements = stationMeasurementsToken.ToObject<List<StationMeasurement>>();

                    if (stationMeasurements != null && stationMeasurements.Count == 0)
                    {
                        throw new Exception("Failed to get data from Api");
                    }

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
