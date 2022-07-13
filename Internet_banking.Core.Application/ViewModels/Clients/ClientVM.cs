using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Clients
{
    public class ClientVM: UserVM
    {
        public List<ProductsVM> Products { get; set; }
    }
}
