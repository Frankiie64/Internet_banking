using Internet_banking.Core.Application.Enums;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Seeds
{
    public class DefaultPrincipalAccount
    {
        public static async Task SeedAsync(ITypeAccountRepository _repo)
        {
            TypeAccount defaulAccount = new();
            defaulAccount.Title = "Cuenta Principal";

            var items = await _repo.GetAllAsync();

            if (items.All(user => user.Title != defaulAccount.Title))
            {
                var account = await _repo.createAsync(defaulAccount);
            }
        }
    }
}
