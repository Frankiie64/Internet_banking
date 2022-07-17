using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Identity.Seeds
{
    public class DefaultTransantionsTable
    {
        public static async Task SeedAsync(ITransactionRepository _repo)
        {
            Transactional Trasansition = new();
            Trasansition.Count_transactional = 0;
            Trasansition.Paids = 0;          
            Trasansition.date = DateTime.Today;

            var items = await _repo.GetAllAsync();

            if (items.All(trasation => trasation.date != Trasansition.date))
            {
                var transactional = await _repo.createAsync(Trasansition);
            }

        }
    }
}
