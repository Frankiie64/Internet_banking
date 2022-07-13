using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.helper
{
    public sealed class SingletonRepository
    {
        public static SingletonRepository Instance { get; } = new SingletonRepository();

        public SaveUserVM client = new();
        public SaveProductVM Product = new();
        public string origin = string.Empty;

        private SingletonRepository()
        { }

    }
}
