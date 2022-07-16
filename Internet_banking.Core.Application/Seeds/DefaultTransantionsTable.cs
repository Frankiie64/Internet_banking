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
            public static async Task SeedAsync()
            {
                Transactional defaultUser = new();
                defaultUser.Count_transactional = 0;
                defaultUser.Paids = 0;
                defaultUser.UserActives = 0;
                defaultUser.UserInactives = 0;
                defaultUser.Count_transactional = 0;

            }
    }
}
