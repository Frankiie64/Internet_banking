using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Transational
{
    public class TransationalVM
    {
        public int Id { get; set; }
        public int Count_transactional { get; set; }
        public int Count_transactional_History { get; set; }
        public int Paids { get; set; }
        public int Paids_History { get; set; }
        public int UserActives { get; set; }
        public int UserInactives { get; set; }
        public int CountProduct { get; set; }
        public DateTime date { get; set; }
    }
}
