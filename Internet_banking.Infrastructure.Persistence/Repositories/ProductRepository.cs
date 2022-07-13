﻿using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Domain.Entities;
using Internet_banking.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Persistence.Repositories
{
    public class ProductRepository: GenericRepository<Products>, IGenericRepository<Products>
    {
        private readonly ApplicationDbContext db;

        public ProductRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }
    }
}
