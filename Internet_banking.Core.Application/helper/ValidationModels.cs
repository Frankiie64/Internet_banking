using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.helper
{
    public static class ValidationModels
    {
        private static readonly Regex regex = new Regex("^[a-zA-Z0-9]*$");

        public static bool IsValid(string str)
        {
            if (regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
