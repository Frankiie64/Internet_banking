using Internet_banking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Domain.Entities
{
    public class TypeAccount:AuditableBaseEntity
    {
        public string Title { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
