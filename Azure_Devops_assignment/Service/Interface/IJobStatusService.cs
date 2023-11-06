using Azure_Devops_assignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IJobStatusService
{
    Task<JobStatusEntity> GetJobStatusAsync(string partitionKey, string rowKey);
    Task<IEnumerable<JobStatusEntity>> GetAllJobStatuses(string partitionKey);
    Task InsertJobStatusAsync(JobStatusEntity entity);
    Task UpdateJobStatusAsync(JobStatusEntity entity);
    Task DeleteJobStatusAsync(string partitionKey, string rowKey);
}
