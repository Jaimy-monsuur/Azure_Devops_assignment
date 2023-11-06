using Azure_Devops_assignment.Model.Enum;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Devops_assignment.Model
{
    public class JobStatusEntity : TableEntity
    {
        public string Status { get; set; }

        public JobStatusEntity() { }

        public JobStatusEntity(string jobId, string timestamp, JobStatus status)
        {
            Status = status.ToString(); // Store the enum as a string.

            RowKey = jobId;
            PartitionKey = timestamp;
        }

        // Add a method to parse the stored string back to the enum.
        public JobStatus GetStatusEnum()
        {
            if (JobStatus.TryParse(Status, out JobStatus status))
            {
                return status;
            }
            // Handle the case where the string couldn't be parsed to an enum.
            return JobStatus.Unknown; // You can choose an appropriate default value.
        }
    }
}
