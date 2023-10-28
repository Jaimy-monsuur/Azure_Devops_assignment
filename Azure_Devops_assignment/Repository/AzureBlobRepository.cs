using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure_Devops_assignment.Repository.Interface;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Azure.Storage;
using Microsoft.Extensions.Logging;

public class AzureBlobRepository : IAzureBlobRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    private BlobContainerClient blobContainerClient;
    private BlobClient blobClient;
    private readonly ILogger _logger;

    public AzureBlobRepository(ILoggerFactory loggerFactory)
    {
        string? connectionString = Environment.GetEnvironmentVariable("BlobConnectionString");
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = loggerFactory.CreateLogger<AzureBlobRepository>();
    }

    public async Task SetBlobContainerClientOrCreateAsync(string containerClientPath)
    {
        blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerClientPath);
        await blobContainerClient.CreateIfNotExistsAsync();
        await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.None);
        _logger.LogInformation("Blob container had been set for: " + containerClientPath);
    }

    public async Task UploadBlobAsync(byte[] content, string blobName)
    {
        blobClient = blobContainerClient.GetBlobClient(blobName);

        using (MemoryStream stream = new MemoryStream(content))
        {
            await blobClient.UploadAsync(stream, true);
        }
        _logger.LogInformation("blob file had been uploaden with name: " + blobName);
    }

    private Uri GetBlobContainerSasToken()
    {
        if (!blobContainerClient.CanGenerateSasUri)
        {
            _logger.LogCritical("Blob client can net generate sas uri");
        }
        BlobSasBuilder builder = new BlobSasBuilder();
        builder.BlobContainerName = blobContainerClient.Name;
        builder.SetPermissions(BlobAccountSasPermissions.Read | BlobAccountSasPermissions.List);
        builder.StartsOn = DateTimeOffset.Now.AddDays(-1);//other wise it just wont work in my time zone :(
        builder.ExpiresOn = DateTimeOffset.Now.AddDays(1);

        return blobContainerClient.GenerateSasUri(builder);
    }

    public  Uri GetBlobContainerUrlWithSasToken()
    {
        var SasURL = GetBlobContainerSasToken();
        _logger.LogInformation("SAS token had been made");
        return SasURL;
    }

    public async Task SetBlobContainerClientAsync(string containerClientPath)
    {
        blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerClientPath);
        await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.None);
        _logger.LogInformation("Blob container had been set for: " + containerClientPath);
    }

    public async Task<List<string>> GetBlobNamesAsync()
    {
        var blobNames = new List<string>();

        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {
            blobNames.Add(blobItem.Name);
        }

        return blobNames;
    }
}
