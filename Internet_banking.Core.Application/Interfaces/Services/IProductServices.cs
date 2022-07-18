using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface IProductServices : IGenericServices<SaveProductVM, ProductsVM, Products>
    {
        public Task<List<ProductsVM>> GetAllWithIncludeAsync();
        Task<bool> Exist(int code);

        Task<SaveProductVM> GetByIdSAsync(string id);

        Task<SaveProductVM> GetProductByCode(int code);


    }
}
