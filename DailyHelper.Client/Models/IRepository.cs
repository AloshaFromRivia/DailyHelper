using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyHelper.Client.Models
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<T> GetAsync(Guid id);
        Task<bool> AddAsync(T item);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Guid id, T item);
    }
}