using Internet_banking.Core.Application.ViewModels.TypeAccount;
using Internet_banking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Products
{
    public class ProductsVM
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int IdAccount { get; set; }
        public TypeAccountVM TypeAccount { get; set; }
        public double Amount { get; set; }
        public string IdClient { get; set; }
        public UserVM client { get; set; }
        public double Paid { get; set; } = 0;

    }
}
