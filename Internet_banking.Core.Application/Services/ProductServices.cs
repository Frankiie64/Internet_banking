using AutoMapper;
using Internet_banking.Core.Application.Enums;
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
        private readonly IUserService _userService;

        private readonly IMapper mapper;
        public ProductServices(IProductRepository repo, IUserService userService, IMapper mapper) : base(repo, mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
            _userService = userService;
        }

        public async override Task<SaveProductVM> CreateAsync(SaveProductVM vm)
        {
            vm.Code = await GenerateCode(2);
            if (vm.IdAccount == (int)TypesAccountEnum.Prestamo)
            {
                var accounts = await repo.GetAllAsync();

                var AccountPricipal = accounts.Where(x => x.IdAccount == (int)TypesAccountEnum.CuentaPrincipal && x.IdClient == vm.IdClient).SingleOrDefault();

                AccountPricipal.Amount += vm.Amount;

                await repo.UpdateAsync(AccountPricipal,AccountPricipal.Id);

            }
            return await base.CreateAsync(vm);
        }
   
        public async override Task<SaveProductVM> DeleteAsync(int id)
        {
            SaveProductVM vm = new SaveProductVM();
            vm.HasError = false;

            var item = await repo.GetByIdAsync(id);

            if (item.IdAccount == (int)TypesAccountEnum.CuentaPrincipal)
            {
                vm.HasError = true;
                vm.Error = "No puedes eliminar la cuenta principal de un usuario";
                return vm;
            }
            if (item.IdAccount == (int)TypesAccountEnum.Cuentadeahorro)
            {
                var list = await repo.GetAllAsync();

                var accountPrincipal = list.Where(pr => pr.IdClient == item.IdClient && pr.IdAccount == (int)TypesAccountEnum.CuentaPrincipal).FirstOrDefault();

                accountPrincipal.Amount += item.Amount;
                               
            }

            if (item.IdAccount == (int)TypesAccountEnum.Tarjetadecredito )
            {
                if (item.Paid != 0 )
                {
                    vm.HasError = true;
                    vm.Error = "Este usuario no podra eliminar esta tarjeta de credito hasta que pague lo que debe.";
                    return vm;
                }           
            }

            if (item.IdAccount == (int)TypesAccountEnum.Prestamo)
            {
                if (item.Amount != item.Paid)
                {
                    vm.HasError = true;
                    vm.Error = "Este usuario no podra eliminar este producto  hasta que pague lo que debe.";
                    return vm;
                }
            }

           var value = await base.DeleteAsync(id);

            if (value != null)
            {
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando eliminar este producto.";
                return vm;
            }

            return vm;
        }

        public async Task<bool> Exist(int code)
        {
            return await repo.ExistAync(code);

        }

        public async Task<SaveProductVM> GetByIdSAsync(string id)
        {
            var entity = await repo.GetByIdAsync(id);
            SaveProductVM vm = mapper.Map<SaveProductVM>(entity);
            return vm;
        }

        //public async Task<SaveUserVM> GetSaveUserVMByIdAsync(string id)
        //{
        //    var item = mapper.Map<SaveUserVM>(await accountServices.GetUserByIdAsync(id));
        //    return item;
        //}


        public async Task<List<ProductsVM>> GetAllWithIncludeAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var list = await repo.GetAllWithIncludeAsync(new List<string>{ "TypeAccount" });
            var listMapped = mapper.Map<List<ProductsVM>>(list);

            foreach (var item in listMapped)
            {
                item.client = users.Where(x => x.Id == item.IdClient).SingleOrDefault();
            }

            return listMapped;

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
        public async Task<SaveProductVM> GetProductByCode(int code)
        {
            var list = mapper.Map<List<SaveProductVM>>(await repo.GetAllAsync());
            var item = list.Where(pr => pr.Code == code).SingleOrDefault();
            return item;
        }

     
    }
}


