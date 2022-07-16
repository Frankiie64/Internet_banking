using Internet_banking.Core.Application.ViewModels.TypeAccount;
using Internet_banking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Clients
{
    public class BeneficiarioVM
    {
        public string IdClient { get; set; }
        public UserVM client { get; set; }

        public int IdAccount { get; set; }
        public TypeAccountVM TypeAccount { get; set; }

    }
}
