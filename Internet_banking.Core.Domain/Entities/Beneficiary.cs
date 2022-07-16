using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Domain.Entities
{
    public class Beneficiary
    {
        public int Id { get; set; }
        public string UserId { get;set; } // My ID
        public int BeneficiaryCode { get; set; }

    }
}
