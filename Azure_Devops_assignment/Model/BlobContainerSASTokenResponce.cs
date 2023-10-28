using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Model
{
    public class BlobContainerSASTokenResponce
    {
        public string BaseSASUri {  get; set; }
        public string ListFilesUri { get; set; }
        public List<string> ContainerFilesUri { get; set; }

        public BlobContainerSASTokenResponce(string baseUri, List<string> filenames)
        {
            BaseSASUri = baseUri;
            ListFilesUri = BaseSASUri + "&restype=container&comp=list";
            ContainerFilesUri = FileNamesToSASUriList(filenames);
        }

        private List<string> FileNamesToSASUriList(List<string> filenames)
        {
            string[] parts = BaseSASUri.Split(new[] { '?' }, 2);
            List<string> SASUri = new List<string>();
            foreach (string name in filenames) 
            {
                SASUri.Add(parts[0] + "/" + name + "?" + parts[1]);
            }
            return SASUri;
        }
    }
}
