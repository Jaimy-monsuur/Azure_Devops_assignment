using Azure_Devops_assignment.Service;
using Azure_Devops_assignment.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(configureServices =>
    {
        configureServices.AddTransient<IWeatherDataService, WeatherDataService>();
        configureServices.AddTransient<IImageService, ImageService>();

    })
    .Build();

host.Run();
