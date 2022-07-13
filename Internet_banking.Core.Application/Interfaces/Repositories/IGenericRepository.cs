using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> createAsync(T entity);
        public Task<List<T>> GetAllAsync();
        public Task<List<T>> GetAllWithIncludeAsync(List<string> properties);
        public Task<T> GetByIdAsync(int id);
        public Task<bool> UpdateAsync(T entity, int id);
        public Task<bool> DeleteAsync(T entity);

    }
}
