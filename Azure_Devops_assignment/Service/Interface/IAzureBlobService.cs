using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Service.Interface
{
    public interface IAzureBlobService
    {
        Task SetBlobContainerClientAsync(string containerClientPath);
        Task SetBlobContainerClientOrCreateAsync(string containerClientPath);
        Task UploadBlobAsync(byte[] content, string blobName);
        Uri GetBlobContainerUrlWithSasToken();
        Task<List<string>> GetBlobNamesAsync();
    }
}
