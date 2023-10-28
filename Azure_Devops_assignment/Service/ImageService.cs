using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Service.Interface;
using ImageMagick;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Service
{
    public class ImageService : IImageService
    {
        private readonly string? _ApiUrl;
        private readonly ILogger _logger;

        public ImageService(ILoggerFactory loggerFactory) 
        {
            _ApiUrl = Environment.GetEnvironmentVariable("ImageApi");
            _logger = loggerFactory.CreateLogger<WeatherDataService>();
        }

        public async Task<byte[]> GetImageForStationMeasurementAsync(StationMeasurement stationMeasurement)
        {
            byte[] image = await GetImageAsByteArrayAsync();
            byte[] imageWithData = WriteStationMeasurementsOnImage(image, stationMeasurement);
            return imageWithData;
        }

        private byte[] WriteStationMeasurementsOnImage(byte[] array, StationMeasurement stationMeasurement) 
        {
            try
            {
                using MemoryStream stream = new MemoryStream(array);
                using MagickImage image = new MagickImage(stream);

                image.Settings.FontWeight = FontWeight.Bold;
                image.Settings.FontPointsize = 17;
                image.Settings.FillColor = MagickColors.White;
                image.Settings.BorderColor = MagickColors.White;

                DrawableText date = new DrawableText(40, 70, $"Date: {DateTime.Now.ToString("dd-MM-yyyy")}");
                DrawableText stationName = new DrawableText(40, 100, $"Station: {stationMeasurement.stationname}");
                DrawableText temperature = new DrawableText(40, 130, $"Regio: {stationMeasurement.temperature}");
                DrawableText feeltemperature = new DrawableText(40, 160, $"Feel temperature: {stationMeasurement.feeltemperature}");
                DrawableText windDirection = new DrawableText(40, 190, $"WindDirection: {stationMeasurement.winddirection}");
                DrawableText windSpeed = new DrawableText(40, 220, $"Windspeed: {stationMeasurement.windspeed}");
                DrawableText humidity = new DrawableText(40, 250, $"Humidity: {stationMeasurement.humidity}");

                image.Draw(date);
                image.Draw(stationName);
                image.Draw(temperature);
                image.Draw(feeltemperature);
                image.Draw(windDirection);
                image.Draw(windSpeed);
                image.Draw(humidity);

                _logger.LogInformation("StationMeasurement have been written on an image");
                return image.ToByteArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<byte[]> GetImageAsByteArrayAsync()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(_ApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                        return imageBytes;
                    }
                    else
                    {
                        throw new Exception("Failed to get image from Api");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
