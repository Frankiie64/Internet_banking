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
    }
}


