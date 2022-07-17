using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Domain.Entities
{
    public class Transactional
    {
        public int Id { get; set; }
        public int Count_transactional { get; set; }
        public int Paids { get; set; }
        public DateTime date { get; set; }
    }
}
