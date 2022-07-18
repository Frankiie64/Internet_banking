using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Domain.Entities;
using Internet_banking.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Persistence.Repositories
{
    public class ProductRepository: GenericRepository<Products>, IProductRepository
    {
        private readonly ApplicationDbContext db;

        public ProductRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }
        public override async Task<bool> UpdateAsync(Products model, int id)
        {
            var product = db.Products.SingleOrDefault(item => item.Id == id);

            model.Creadted = product.Creadted;
            model.CreatedBy = product.CreatedBy;

            return await base.UpdateAsync(model, id);

        }


        public async Task<bool> ExistAync(int code)
        {
            return await db.Set<Products>().AnyAsync(c => c.Code == code);
        }

        public async Task<Products> GetByIdAsync(string id)
        {
            return await db.Set<Products>().FindAsync(id);

        }
    }
}
