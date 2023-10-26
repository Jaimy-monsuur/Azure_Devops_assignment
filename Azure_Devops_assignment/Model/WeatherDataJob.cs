using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Model
{
    public class WeatherDataJob
    {
        public string? JobId { get; set; }
        public string Timestamp { get; set; }
        public StationMeasurement Measurement { get; set; }

        public WeatherDataJob(string jobData, string timestamp, StationMeasurement measurement)
        {
            this.JobId = jobData;
            this.Timestamp = timestamp;
            this.Measurement = measurement;
        }
    }
}
