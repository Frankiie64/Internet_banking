using AutoMapper;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Clients.Beneficiary;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Services
{
    public class BeneficiaryServices : GenericServices<SaveBeneficiaryVM ,BeneficiaryVM, Beneficiary>, IBeneficiaryServices
    {
        private readonly IBeneficiaryRepository repo;
        private readonly IMapper mapper;
        private readonly IUserService _userService;
        private readonly IProductServices _productService;


        public BeneficiaryServices(IBeneficiaryRepository repo, IUserService userService, IProductServices productService, IMapper mapper) : base(repo, mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
            _userService = userService;
            _productService = productService;
        }



        public override async Task<List<BeneficiaryVM>> GetAllViewModelAsync()
        {
            var list = mapper.Map<List<BeneficiaryVM>>(await repo.GetAllAsync());
            var products = await _productService.GetAllWithIncludeAsync();
            var users = await _userService.GetAllUsersAsync();

            foreach (var item in list)
            {
                item.User = users.Where(x => x.Id == item.UserId).FirstOrDefault();
                item.Beneficiary = products.Where(x => x.Code == item.BeneficiaryCode).FirstOrDefault();
            }

            return list;
        }

        public async Task<SaveBeneficiaryVM> GetByIdSAsync(int id)
        {
            Beneficiary entity = await repo.GetByIdAsync(id);
            SaveBeneficiaryVM vm = mapper.Map<SaveBeneficiaryVM>(entity);
            return vm;
        }
    }
}
