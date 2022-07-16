using Internet_banking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Domain.Entities
{
    public class Products : AuditableBaseEntity
    {
        public int IdAccount { get; set; }
        public TypeAccount TypeAccount { get; set; }
        public double Amount { get; set; }
        public string IdClient { get; set; }
        public int Code { get; set; }
        public double Paid { get; set; } = 0;

    }
}
