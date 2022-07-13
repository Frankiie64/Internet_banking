using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.ViewModels.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPassWordResponse> ForgotPasswordAsync(ForgotPasswordVM vm, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginVM vm);
        Task<RegisterResponse> RegisterAsync(SaveUserVM vm,string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordVM vm);
        Task<List<UserVM>> GetAllUsersAsync();
        Task<UserVM> GetUserByIdAsync(string id);
        Task SignOutAsync();
    }
}