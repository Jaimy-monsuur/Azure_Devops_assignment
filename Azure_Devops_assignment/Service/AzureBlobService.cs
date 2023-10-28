using System;
using System.IO;
using System.Threading.Tasks;
using Azure_Devops_assignment.Service.Interface;
using Azure_Devops_assignment.Repository.Interface;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;

namespace Azure_Devops_assignment.Service
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IAzureBlobRepository _blobRepository;
        private readonly ILogger _logger;

        public AzureBlobService(IAzureBlobRepository blobRepository, ILoggerFactory loggerFactory)
        {
            _blobRepository = blobRepository;
            _logger = loggerFactory.CreateLogger<AzureBlobService>();
        }

        public async Task SetBlobContainerClientAsync(string containerClientPath)
        {
            _logger.LogInformation("Set blob container for: " + containerClientPath);
            await _blobRepository.SetBlobContainerClientAsync(containerClientPath);
        }

        public async Task UploadBlobAsync(byte[] content, string blobName)
        {
            _logger.LogInformation("Upload blob file: " + blobName);
            await _blobRepository.UploadBlobAsync(content, blobName);
        }

        public Uri GetBlobContainerUrlWithSasToken()
        {
            _logger.LogInformation("Get blob container SAS token");
            return _blobRepository.GetBlobContainerUrlWithSasToken();
        }

        public async Task SetBlobContainerClientOrCreateAsync(string containerClientPath)
        {
            _logger.LogInformation("Set blob container for: " + containerClientPath);
            await _blobRepository.SetBlobContainerClientOrCreateAsync(containerClientPath);
        }

        public async Task<List<string>> GetBlobNamesAsync()
        {
            _logger.LogInformation("Getting all names from container");
            return await _blobRepository.GetBlobNamesAsync();
        }
    }
}
