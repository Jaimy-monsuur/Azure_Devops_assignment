using Azure_Devops_assignment.Model;

public class JobStatusService : IJobStatusService
{
    private readonly ITableStorageRepository<JobStatusEntity> repository;

    public JobStatusService(ITableStorageRepository<JobStatusEntity> repository)
    {
        this.repository = repository;
    }

    public async Task<JobStatusEntity> GetJobStatusAsync(string partitionKey, string rowKey)
    {
        return await repository.GetAsync(partitionKey, rowKey);
    }

    public async Task<IEnumerable<JobStatusEntity>> GetAllJobStatuses(string partitionKey)
    {
        return await repository.GetAllAsync(partitionKey);
    }

    public async Task InsertJobStatusAsync(JobStatusEntity entity)
    {
        await repository.InsertAsync(entity);
    }

    public async Task UpdateJobStatusAsync(JobStatusEntity entity)
    {
        await repository.UpdateAsync(entity);
    }

    public async Task DeleteJobStatusAsync(string partitionKey, string rowKey)
    {
        await repository.DeleteAsync(partitionKey, rowKey);
    }
}
