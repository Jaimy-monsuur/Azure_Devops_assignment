using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Repository.Interface
{
    public interface IAzureBlobRepository
    {
        Task UploadBlobAsync(byte[] content, string blobPath);
        Task SetBlobContainerClientAsync(string containerClientPath);
        Task SetBlobContainerClientOrCreateAsync(string containerClientPath);
        Uri GetBlobContainerUrlWithSasToken();
        Task<List<string>> GetBlobNamesAsync();
    }
}
