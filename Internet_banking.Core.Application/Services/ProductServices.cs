using AutoMapper;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Services
{
    public class ProductServices:GenericServices<SaveProductVM,ProductsVM,Products>, IProductServices
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;
        public ProductServices(IProductRepository repo, IMapper mapper) : base(repo, mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<SaveProductVM> CreateAsync(SaveProductVM vm)
        {
            vm.Code = await GenerateCode(2);
            return await base.CreateAsync(vm);
        }
        private async Task<int> GenerateCode(int counter)
        {
            int code = 0;
            for (int i = 0; i < counter; i++)
            {
                Random random = new();

                code = random.Next(100000000, 999999999);

                var list = await repo.GetAllAsync();

                foreach (var item in list)
                {
                    if (item.Code == code)
                    {
                        counter += 1;
                    }
                }
            }
            return code;
        }

    }
}


