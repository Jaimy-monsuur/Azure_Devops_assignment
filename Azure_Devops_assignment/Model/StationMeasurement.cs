using System;

namespace Azure_Devops_assignment.Model
{
    public class StationMeasurement
    {
        public string? Id { get; set; }
        public int stationid { get; set; }
        public string? stationname { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string? regio { get; set; }
        public DateTime timestamp { get; set; }
        public string? weatherdescription { get; set; }
        public string? iconurl { get; set; }
        public string? fullIconUrl { get; set; }
        public string? graphUrl { get; set; }
        public string? winddirection { get; set; }
        public double temperature { get; set; }
        public double groundtemperature { get; set; }
        public double feeltemperature { get; set; }
        public double windgusts { get; set; }
        public double windspeed { get; set; }
        public int windspeedBft { get; set; }
        public double humidity { get; set; }
        public double precipitation { get; set; }
        public double sunpower { get; set; }
        public double rainFallLast24Hour { get; set; }
        public double rainFallLastHour { get; set; }
        public double winddirectiondegrees { get; set; }
    }
}
