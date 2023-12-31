using Azure_Devops_assignment.Model;
using Azure_Devops_assignment.Repository.Interface;
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
        configureServices.AddTransient<IAzureBlobService, AzureBlobService>();
        configureServices.AddTransient<IAzureBlobRepository, AzureBlobRepository>();
        configureServices.AddTransient<IJobStatusService, JobStatusService>();
        configureServices.AddSingleton<ITableStorageRepository<JobStatusEntity>, TableStorageRepository<JobStatusEntity>>(provider => 
        {
            string tableName = "JobRequestTable";
            return new TableStorageRepository<JobStatusEntity>(tableName);
        });
    })
    .Build();

host.Run();

