using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITableStorageRepository<T> where T : class
{
    Task<T> GetAsync(string partitionKey, string rowKey);
    Task<IEnumerable<T>> GetAllAsync(string partitionKey);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string partitionKey, string rowKey);
}
