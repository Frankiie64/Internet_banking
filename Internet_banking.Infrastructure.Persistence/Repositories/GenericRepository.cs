using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;

        public GenericRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<T> createAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();

            return entity;
        }
        public virtual async Task<bool> DeleteAsync(T entity)
        {
            db.Set<T>().Remove(entity);
            return await Save();
        }
        public virtual async Task<bool> UpdateAsync(T entity, int id)
        {
            T entry = await db.Set<T>().FindAsync(id);
            db.Entry(entry).CurrentValues.SetValues(entity);
            return await Save();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await db.Set<T>().ToListAsync();
        }
        public virtual async Task<List<T>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = db.Set<T>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }
        private async Task<bool> Save()
        {
            return await db.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
