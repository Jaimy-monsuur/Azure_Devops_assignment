using Azure_Devops_assignment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Service.Interface
{
    public interface IWeatherDataService
    {
        public Task<List<StationMeasurement>> GetWeatherDataFromApiAsync();
        public List<WeatherDataJob> GetWeatherDataJobs(string jobid, string timestamp);
        public Task<byte[]> GetImageAsByteArrayAsync();
    }
}
