using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Model
{
    public class JobRequest
    {
        public string? JobRequestId { get; set; }
        public string? Timestamp { get; set; }
        public string? BaseUrl { get; set; }
        public string? JobRequestUrl { get; set; }
    }
}
