using AutoMapper;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Transational;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Services
{
    public class TrasantionalService : GenericServices<SaveTransationalVM, TransationalVM, Transactional>, ITrasantionalService
    {
        private readonly IProductRepository ProductRepo;
        private readonly ITransactionRepository repo;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public TrasantionalService(ITransactionRepository repo,  IMapper mapper, IUserService userService, IProductRepository ProductRepo) : base(repo, mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userService = userService;
            this.ProductRepo = ProductRepo;
        }

        public async Task<SaveTransationalVM> GetByDateTrasations()
        {
         
            List<SaveTransationalVM> list = mapper.Map<List<SaveTransationalVM>>(await repo.GetAllAsync());

            var item = list.Where(trasantions => trasantions.date == DateTime.Today).SingleOrDefault();

            return item;
        }

        public async Task<TransationalVM> GetTransationalToday()
        {
            var clients = await userService.GetAllClientsAsync();
            var products = await ProductRepo.GetAllAsync();

            List<TransationalVM> list = mapper.Map<List<TransationalVM>>(await repo.GetAllAsync());

            var item = list.Where(trasantions => trasantions.date == DateTime.Today).SingleOrDefault();


            foreach (var transation in list)
            {
                item.Count_transactional_History += transation.Count_transactional;
                item.Paids_History += transation.Paids;
            }

            item.UserActives = clients.Where(client => client.IsVerified == true).Count();
            item.UserInactives = clients.Where(client => client.IsVerified == false).Count();
            item.CountProduct = products.Count();

            return item;
        }


    }
}
