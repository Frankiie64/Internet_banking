using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Domain.Entities;
using Internet_banking.Infrastructure.Persistence.Context;

namespace Internet_banking.Infrastructure.Persistence.Repositories
{
   public class TransactionRepository : GenericRepository<Transactional>, ITransactionRepository
    {
        private readonly ApplicationDbContext db;

        public TransactionRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }
    }
}
