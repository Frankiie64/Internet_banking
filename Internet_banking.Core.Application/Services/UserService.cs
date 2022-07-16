using AutoMapper;
using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Enums;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Users;
using Internet_banking.Core.Application.ViewModels.Users.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountServices accountServices;
        private readonly IMapper mapper;

        public UserService(IAccountServices accountServices, IMapper mapper)
        {
            this.accountServices = accountServices;
            this.mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginVM vm)
        {
            AuthenticationRequest LoginRequest = mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse response = await accountServices.AuthenticationAsync(LoginRequest);

            return response;
        }
        public async Task SignOutAsync()
        {
            await accountServices.SignOutAsync();
        }
        public async Task<RegisterResponse>  RegisterAsync(SaveUserVM vm, string origin)
        {
            RegisterRequest registerRequest = mapper.Map<RegisterRequest>(vm);
            return await accountServices.RegisterUserAsync(registerRequest, origin);
        }
        public async Task<RegisterResponse> UpdateUserAsync(SaveUserVM vm)
        {
            RegisterRequest registerRequest = mapper.Map<RegisterRequest>(vm);
            return await accountServices.UpdateUserAsync(registerRequest);
        }
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await accountServices.ConfirmEmailAsync(userId, token);
        }
        public async Task<ForgotPassWordResponse> ForgotPasswordAsync(ForgotPasswordVM vm, string origin)
        {
            ForgotPassowordRequest forgotRequest = mapper.Map<ForgotPassowordRequest>(vm);
            return await accountServices.ForgotPasswordRequestAsync(forgotRequest, origin);
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordVM vm)
        {
            ResetPasswordRequest resetRequest = mapper.Map<ResetPasswordRequest>(vm);            
            return await accountServices.ResetPasswordAsync(resetRequest);
        }
        public async Task<List<UserVM>> GetAllUsersAsync()
        {
            var items = mapper.Map<List<UserVM>>( await accountServices.GetAllUsersAsync());

            foreach (var item in items)
            {
                if (item.Roles.Count() == 1)
                {
                    foreach (var ls in item.Roles)
                    {
                        item.IsClient = ls == Roles.Admin.ToString() ? false : true;
                    }
                }
            }
            return items;
        }
        public async Task<UserVM> GetUserByIdAsync(string id)
        {
            return mapper.Map<UserVM>(await accountServices.GetUserByIdAsync(id));
        }
        public async Task<SaveUserVM> GetSaveUserVMByIdAsync(string id)
        {
            var item = mapper.Map<SaveUserVM>(await accountServices.GetUserByIdAsync(id));
            return item;
        }
        public async Task<SaveClienteVM> GetSaveClientVMByIdAsync(string id)
        {
            var item = mapper.Map<SaveClienteVM>(await accountServices.GetUserByIdAsync(id));
            return item;
        }
        public async Task<List<UserVM>> GetAllClientsAsync()
        {
            List<UserVM> items = mapper.Map<List<UserVM>>(await accountServices.GetAllUsersAsync());

            items = items.Where(clients => clients.Roles[0] == "Cliente").ToList();

            return items;
        }
        public Task<AuthenticationResponse> IsVerified(string id)
        {
            return accountServices.IsVerified(id);
        }
    }
}
